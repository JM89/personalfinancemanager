import test from "node:test";
import assert from "node:assert/strict";

const base = process.env.PFM_OFFLINE_URL;

test("offline API uses the existing in-memory implementations", { skip: !base }, async () => {
  const deepLink = await fetch(`${base}/expenses/list/1`);
  assert.equal(deepLink.status, 200);
  assert.match(await deepLink.text(), /<div id="root">/);
  const mode = await fetch(`${base}/api/frontend/mode`);
  assert.deepEqual(await mode.json(), { offline: true });
  assert.equal((await fetch(`${base}/api/frontend/does-not-exist`)).status, 404);
  const anonymous = await fetch(`${base}/api/frontend/accounts`, { redirect: "manual" });
  assert.equal(anonymous.status, 401);
  const login = await fetch(`${base}/login?redirectUri=/`, { redirect: "manual" });
  assert.equal(login.status, 302);
  const cookie = login.headers.get("set-cookie").split(";")[0];
  const api = async (path, method = "GET", body) => {
    const headers = { Cookie: cookie };
    if (body && !(body instanceof FormData)) {
      headers["Content-Type"] = "application/json";
      body = JSON.stringify(body);
    }
    const response = await fetch(`${base}/api/frontend${path}`, { method, headers, body });
    // Development error bodies may include request headers, including the session cookie.
    assert.ok(response.ok, `${method} ${path}: ${response.status}`);
    const text = await response.text();
    return text ? JSON.parse(text) : null;
  };
  const suffix = Date.now();
  const name = `Test category ${suffix}`;
  const hostile = await fetch(`${base}/api/frontend/expense-types`, {
    method: "POST",
    headers: { Cookie: cookie, Origin: "https://example.com", "Content-Type": "application/json" },
    body: JSON.stringify({ name, graphColor: "123ABC", showOnDashboard: true }),
  });
  assert.equal(hostile.status, 403);
  const allowedOriginPort = process.env.PFM_OFFLINE_TEST_ORIGIN_PORT ?? "5174";
  for (const [origin, expected] of [
    [`http://localhost:${allowedOriginPort}`, 400],
    [`http://127.0.0.1:${allowedOriginPort}`, 400],
    ["http://localhost:65535", 403],
    [`http://example.com:${allowedOriginPort}`, 403],
  ]) {
    const response = await fetch(`${base}/api/frontend/expense-types`, {
      method: "POST",
      headers: { Cookie: cookie, Origin: origin, "Content-Type": "application/json" },
      body: "{",
    });
    assert.equal(response.status, expected, origin);
  }

  const bankName = `Test bank ${suffix}`;
  await api("/accounts");
  const form = new FormData();
  form.set("name", bankName);
  form.set("countryId", "1");
  form.set("website", "https://example.com");
  form.set("generalEnquiryPhoneNumber", "00000000000");
  form.set("icon", new Blob([Buffer.from("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAADUlEQVQIHWP4z8DwHwAFgAI/ScLttAAAAABJRU5ErkJggg==", "base64")], { type: "image/png" }), "icon.png");
  await api("/banks", "POST", form);
  const bank = (await api("/banks")).find((item) => item.name === bankName);
  assert.ok(bank);
  await api("/accounts", "POST", { name: bankName, bankId: bank.id, currencyId: 1, initialBalance: 0, isSavingAccount: true });
  const account = (await api("/accounts")).find((item) => item.name === bankName);
  assert.equal(account.bankName, bankName);
  const editedBank = new FormData();
  editedBank.set("name", `${bankName} renamed`);
  editedBank.set("countryId", "1");
  editedBank.set("website", "https://example.com");
  editedBank.set("generalEnquiryPhoneNumber", "00000000000");
  editedBank.set("iconPath", bank.iconPath);
  await api(`/banks/${bank.id}`, "PUT", editedBank);
  assert.equal((await api("/accounts")).find((item) => item.id === account.id).bankName, `${bankName} renamed`);
  const savingDate = "2025-11-17";
  await api("/accounts/1/savings", "POST", { dateSaving: savingDate, amount: 10, targetInternalAccountId: account.id });
  const linkedSaving = (await api("/accounts/1/savings")).at(-1);
  assert.equal(linkedSaving.dateSaving, `${savingDate}T00:00:00`);
  assert.equal(linkedSaving.targetInternalAccountName, bankName);
  await api(`/savings/${linkedSaving.id}`, "DELETE");
  await api(`/accounts/${account.id}`, "DELETE");
  await api(`/banks/${bank.id}`, "DELETE");

  await api("/expense-types", "POST", { name, graphColor: "123ABC", showOnDashboard: true });
  const category = (await api("/expense-types")).find((item) => item.name === name);
  assert.ok(category);
  const date = new Date().toISOString().slice(0, 10);
  await api("/accounts/1/expenses", "POST", { dateExpense: date, description: name, cost: 12, expenseTypeId: 1, paymentMethodId: 1 });
  const expense = (await api("/accounts/1/expenses")).find((item) => item.description === name);
  assert.equal(expense.expenseTypeName, "Groceries");
  const categoryInUse = await fetch(`${base}/api/frontend/expense-types/1`, { method: "DELETE", headers: { Cookie: cookie } });
  assert.equal(categoryInUse.status, 409);
  await api(`/expenses/${expense.id}`, "DELETE");

  await api("/accounts/1/incomes", "POST", { dateIncome: date, description: name, cost: 25 });
  const income = (await api("/accounts/1/incomes")).find((item) => item.description === name);
  assert.ok(income);
  await api(`/incomes/${income.id}`, "DELETE");

  const pensionName = `Pension ${suffix}`;
  await api("/pensions", "POST", { schemeName: pensionName, loginUrl: "https://example.com/pension", currencyId: 1, countryId: 1, currentPot: 12500, currentContribution: 250 });
  let pension = (await api("/pensions")).find((item) => item.schemeName === pensionName);
  assert.ok(pension);
  const pensionDetails = await api(`/pensions/${pension.id}`);
  assert.equal(pensionDetails.currentPot, 12500);
  await api(`/pensions/${pension.id}`, "PUT", { schemeName: `${pensionName} edited`, loginUrl: "", currencyId: 2, countryId: 2, currentPot: 13000, currentContribution: 275 });
  pension = (await api("/pensions")).find((item) => item.id === pension.id);
  assert.equal(pension.schemeName, `${pensionName} edited`);
  assert.equal(pension.currentPot, 13000);
  await api(`/pensions/${pension.id}`, "DELETE");
  assert.equal((await api("/pensions")).some((item) => item.id === pension.id), false);

  const report = { payDay: date, taxableIncome: 42000, incomeTaxPaid: 6200, nationalInsurancePaid: 3200, currencyId: 1, countryId: 1 };
  await api("/income-tax-reports", "POST", report);
  let taxReport = (await api("/income-tax-reports")).find((item) => item.payDay.startsWith(date));
  assert.ok(taxReport);
  const taxDetails = await api(`/income-tax-reports/${taxReport.id}`);
  assert.equal(taxDetails.taxableIncome, 42000);
  await api(`/income-tax-reports/${taxReport.id}`, "PUT", { ...report, payDay: "2025-01-15", taxableIncome: 45000 });
  taxReport = (await api("/income-tax-reports")).find((item) => item.id === taxReport.id);
  assert.equal(taxReport.payDay.slice(0, 10), "2025-01-15");
  assert.equal(taxReport.taxableIncome, 45000);
  await api(`/income-tax-reports/${taxReport.id}`, "DELETE");
  assert.equal((await api("/income-tax-reports")).some((item) => item.id === taxReport.id), false);

  const invalidPension = await fetch(`${base}/api/frontend/pensions`, {
    method: "POST",
    headers: { Cookie: cookie, "Content-Type": "application/json" },
    body: JSON.stringify({ schemeName: "", loginUrl: "javascript:alert(1)", currencyId: 1, countryId: 1, currentPot: 0, currentContribution: 0 }),
  });
  assert.equal(invalidPension.status, 400);

  await api("/accounts/1/savings", "POST", { dateSaving: date, amount: 25, targetInternalAccountId: 5 });
  const savings = await api("/accounts/1/savings");
  const saving = savings.at(-1);
  assert.equal(saving.targetInternalAccountId, 5);
  await api(`/savings/${saving.id}`, "DELETE");

  await api("/accounts/1/withdrawals", "POST", { dateExpense: date, initialAmount: 99 });
  const withdrawal = (await api("/accounts/1/withdrawals")).at(-1);
  const close = await fetch(`${base}/api/frontend/withdrawals/${withdrawal.id}/close`, { method: "POST", headers: { Cookie: cookie } });
  assert.equal(close.status, 400);
  await api(`/withdrawals/${withdrawal.id}`, "DELETE");

  await api("/accounts/1/budgets", "POST", { name, expectedIncomes: 100, expectedSavings: 10, expenseTypes: [{ expenseType: category, expectedValue: 20 }] });
  const budget = (await api("/accounts/1/budgets")).find((item) => item.name === name);
  const details = await api(`/budgets/${budget.id}`);
  details.expectedIncomes = 200;
  details.expectedSavings = 15;
  await api(`/accounts/1/budgets/${budget.id}`, "PUT", details);
  const edited = await api(`/budgets/${budget.id}`);
  assert.equal(edited.expectedIncomes, 100);
  assert.equal(edited.expectedSavings, 10);
  await api(`/accounts/1/budgets/${budget.id}/start`, "POST");
  await api(`/budgets/${budget.id}/stop`, "POST");

  const dashboard = await api("/accounts/1/dashboard");
  assert.ok(dashboard.categories.aggregates);
  assert.ok(dashboard.movements.aggregates);
  assert.ok(dashboard.categoryDiffs.pagedList.list.length);
  const categoryDetail = await api("/accounts/1/dashboard/category?name=Groceries");
  assert.ok(categoryDetail.aggregates);
  assert.ok(Array.isArray(categoryDetail.expenses));
  const budgetContext = await api(`/accounts/1/budget-context?excludeId=${budget.id}`);
  assert.ok(budgetContext.average.aggregates);
  assert.ok(budgetContext.lastMonth.aggregates);
  await api(`/expense-types/${category.id}`, "DELETE");
});
