import React, { useEffect, useState } from "react";
import { createRoot } from "react-dom/client";
import { request } from "./api";
import { CategoryChart, CategoryTrendChart, ExpenseTrendChart, MovementChart } from "./DashboardCharts.jsx";
import "./style.css";

const today = () => new Date().toISOString().slice(0, 10);
const money = (value) => Number(value || 0).toFixed(2);
const date = (value) => value ? value.slice(0, 10) : "";

function Link({ to, children, ...props }) {
  return <a href={to} {...props} onClick={(event) => {
    if (event.button !== 0 || event.metaKey || event.ctrlKey || event.shiftKey || event.altKey) return;
    event.preventDefault();
    history.pushState(null, "", to);
    window.dispatchEvent(new PopStateEvent("popstate"));
  }}>{children}</a>;
}

function usePath() {
  const [path, setPath] = useState(location.pathname);
  useEffect(() => {
    const update = () => setPath(location.pathname);
    window.addEventListener("popstate", update);
    return () => window.removeEventListener("popstate", update);
  }, []);
  return path;
}

function navigate(path) {
  history.pushState(null, "", path);
  window.dispatchEvent(new PopStateEvent("popstate"));
}

function useData(paths, refresh = 0) {
  const [state, setState] = useState({ loading: true, data: null, error: "" });
  const key = paths.join("|");
  useEffect(() => {
    let active = true;
    setState({ loading: true, data: null, error: "" });
    Promise.all(paths.map((path) => request(path)))
      .then((data) => { if (active) setState({ loading: false, data, error: "" }); })
      .catch((error) => { if (active) setState({ loading: false, data: null, error: error.message, status: error.status }); });
    return () => { active = false; };
  }, [key, refresh]);
  return state;
}

function Loader({ state, children }) {
  if (state.loading) return <p>Loading...</p>;
  if (state.error) return <p className="error" role="alert">{state.error}</p>;
  return children(state.data);
}

function Action({ children, onClick, danger = false }) {
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  return <><button type="button" className={danger ? "danger" : "secondary"} disabled={busy} onClick={async () => {
    setError("");
    setBusy(true);
    try { await onClick(); } catch (e) { setError(e.message); } finally { setBusy(false); }
  }}>{children}</button>{error && <span className="error" role="alert">{error}</span>}</>;
}

function Field({ label, name, model, setModel, type = "text", required = false, options, ...props }) {
  const value = model[name] ?? (type === "checkbox" ? false : "");
  const update = (event) => setModel({ ...model, [name]: type === "checkbox" ? event.target.checked : type === "number" || options ? (event.target.value === "" ? null : Number(event.target.value)) : type === "date" && event.target.value === "" ? null : event.target.value });
  return <label className="field">{label}
    {options ? <select value={value} onChange={update} required={required}><option value="">Select...</option>{options.map((item) => <option key={item.id} value={item.id}>{item.name}</option>)}</select>
      : <input {...props} type={type} value={type === "checkbox" ? undefined : type === "date" ? date(value) : value} checked={type === "checkbox" ? value : undefined} onChange={update} required={required} />}
  </label>;
}

function Form({ title, initial, fields, onSave, cancel, children }) {
  const [model, setModel] = useState(initial);
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  return <section><h1>{title}</h1><form className="card form" onSubmit={async (event) => {
    event.preventDefault();
    setError("");
    setBusy(true);
    try { await onSave(model); navigate(cancel); } catch (e) { setError(e.message); } finally { setBusy(false); }
  }}>
    {fields.map((field) => <Field key={field.name} {...field} model={model} setModel={setModel} />)}
    {typeof children === "function" ? children(model, setModel) : children}
    {error && <p className="error" role="alert">{error}</p>}
    <div className="actions"><Link to={cancel}>Cancel</Link><button disabled={busy}>Save</button></div>
  </form></section>;
}

function Table({ columns, rows, actions }) {
  return rows.length === 0 ? <p>No items found.</p> : <div className="table-wrap"><table><thead><tr>{columns.map(([key, title]) => <th key={key}>{title}</th>)}{actions && <th>Actions</th>}</tr></thead><tbody>
    {rows.map((row) => <tr key={row.id}>{columns.map(([key, , render]) => <td key={key}>{render ? render(row) : String(row[key] ?? "")}</td>)}{actions && <td className="row-actions">{actions(row)}</td>}</tr>)}
  </tbody></table></div>;
}

function App() {
  const path = usePath();
  const session = useData(["/session"]);
  if (session.loading) return <main><p>Loading...</p></main>;
  if (session.error) return session.status === 401
    ? <main className="login card"><h1>Personal Finance Manager</h1><p>Offline development mode</p><a className="button" href="/login?redirectUri=/">Log in as local developer</a></main>
    : <main><p className="error" role="alert">Could not connect to the website: {session.error}</p></main>;
  return <div className="layout"><aside><Link className="brand" to="/">PFM <small>Offline</small></Link><nav>
    <Link to="/">Dashboard</Link><Link to="/expense-types">Expense types</Link><Link to="/banks">Banks</Link><Link to="/bank-accounts">Bank accounts</Link>
  </nav></aside><main><header><span>Offline data is reset when the website stops.</span><span>{session.data[0].name} · <a href="/logout">Log out</a></span></header>
    <Page key={path} path={path} />
  </main></div>;
}

function Page({ path }) {
  const parts = path.split("/").filter(Boolean);
  if (!parts.length || (parts.length === 1 && /^\d+$/.test(parts[0]))) return <Dashboard selected={parts[0]} />;
  if (parts[0] === "expense-types") return <ExpenseTypes parts={parts} />;
  if (parts[0] === "banks") return <Banks parts={parts} />;
  if (parts[0] === "bank-accounts") return <Accounts parts={parts} />;
  const kinds = { expenses: "expenses", incomes: "incomes", savings: "savings", "atm-withdraws": "withdrawals", "budget-plans": "budgets" };
  if (kinds[parts[0]] && parts[2]) return <Movements key={path} kind={kinds[parts[0]]} parts={parts} />;
  return <section><h1>Page not found</h1><Link to="/">Go to dashboard</Link></section>;
}

function Dashboard({ selected }) {
  const accounts = useData(["/accounts"]);
  return <Loader state={accounts}>{([list]) => {
    const current = list.find((a) => String(a.id) === selected) || list.find((a) => a.isFavorite && !a.isSavingAccount) || list.find((a) => !a.isSavingAccount);
    return <section><h1>Account Management Dashboard</h1>
      {current ? <><div className="card"><label>Account <select value={current.id} onChange={(e) => navigate(`/${e.target.value}`)}>{list.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}</select></label>
        <p>Balance: {current.displayedCurrentBalance || money(current.currentBalance)}</p>
        <div className="links">{[["Expenses", "expenses"], ["ATM withdrawals", "atm-withdraws"], ["Incomes", "incomes"], ["Savings", "savings"], ["Budget plans", "budget-plans"]].map(([label, slug]) => <Link key={slug} to={`/${slug}/list/${current.id}`}>{label}</Link>)}</div></div>
        <DashboardData accountId={current.id} /></>
        : <p>Create a <Link to="/banks/create">bank</Link> and <Link to="/bank-accounts/create">bank account</Link> to get started.</p>}
    </section>;
  }}</Loader>;
}

function DashboardData({ accountId }) {
  const state = useData([`/accounts/${accountId}/dashboard`, "/expense-types"]);
  const [category, setCategory] = useState(null);
  return <Loader state={state}>{([data, expenseTypes]) => <><div className="stats">
    {Object.entries(data.summary).map(([name, value]) => <div className="card" key={name}><small>{name.replace(/([A-Z])/g, " $1")}</small><strong>{money(value)}</strong></div>)}
  </div><div className="grid"><section className="card"><h2>Expenses by category</h2><CategoryChart aggregates={data.categories.aggregates} expenseTypes={expenseTypes} onSelect={setCategory} /></section>
    <section className="card"><h2>Movement over time</h2><MovementChart aggregates={data.movements.aggregates} /><details><summary>View movement data</summary><div className="table-wrap"><table><thead><tr><th>Month</th><th>Expenses</th><th>Incomes</th><th>Savings</th></tr></thead><tbody>{Object.entries(data.movements.aggregates).map(([month, v]) => <tr key={month}><td>{month}</td><td>{money(v.expensesAmount)}</td><td>{money(v.incomesAmount)}</td><td>{money(v.savingsAmount)}</td></tr>)}</tbody></table></div></details></section>
    <section className="card"><h2>Expenses over time</h2><ExpenseTrendChart aggregates={data.expenses.aggregates} /><details><summary>View expense data</summary><div className="table-wrap"><table><thead><tr><th>Month</th><th>Actual</th><th>Expected</th></tr></thead><tbody>{Object.entries(data.expenses.aggregates).sort(([a], [b]) => a.localeCompare(b)).map(([month, value]) => <tr key={month}><td>{month}</td><td>{money(value.actual)}</td><td>{money(value.expected)}</td></tr>)}</tbody></table></div></details></section>
    <section className="card"><h2>Category comparisons</h2><table><thead><tr><th>Category</th><th>Actual</th><th>Previous</th><th>Average</th></tr></thead><tbody>{data.categoryDiffs.pagedList.list.map((row) => <tr key={row.expenseTypeName}><td><button className="text-button" onClick={() => setCategory(row.expenseTypeName)}>{row.expenseTypeName}</button></td><td>{money(row.actual)}</td><td>{money(row.previousMonth)}</td><td>{money(row.average)}</td></tr>)}</tbody></table></section></div>
    {category && <DashboardCategory key={`${accountId}-${category}`} accountId={accountId} category={category} expenseTypes={expenseTypes} />}</>}</Loader>;
}

function DashboardCategory({ accountId, category, expenseTypes }) {
  const state = useData([`/accounts/${accountId}/dashboard/category?name=${encodeURIComponent(category)}`]);
  return <Loader state={state}>{([data]) => <section className="card"><h2>Details for {category}</h2><div className="grid">
    <div><h3>Expenses over time</h3><CategoryTrendChart aggregates={data.aggregates} category={category} expenseTypes={expenseTypes} /><details><summary>View category totals</summary><div className="table-wrap"><table><thead><tr><th>Month</th><th>Amount</th></tr></thead><tbody>{Object.entries(data.aggregates).sort(([a], [b]) => a.localeCompare(b)).map(([month, value]) => <tr key={month}><td>{month}</td><td>{money(value)}</td></tr>)}</tbody></table></div></details></div>
    <div><h3>Expenses</h3><Table rows={data.expenses} columns={[["dateExpense", "Date", (r) => date(r.dateExpense)], ["description", "Description"], ["cost", "Amount", (r) => money(r.cost)]]} /></div>
  </div></section>}</Loader>;
}

function ExpenseTypes({ parts }) {
  const [refresh, setRefresh] = useState(0);
  const editing = parts[1] === "edit";
  const creating = parts[1] === "create";
  const state = useData(editing ? [`/expense-types/${parts[2]}`] : ["/expense-types"], refresh);
  return <Loader state={state}>{([data]) => creating || editing
    ? <Form key={parts.join("/")} title={`${editing ? "Edit" : "Create"} expense type`} initial={editing ? data : { name: "", graphColor: "3399FF", showOnDashboard: true }} cancel="/expense-types"
      fields={[{ name: "name", label: "Name", required: true, maxLength: 50 }, { name: "graphColor", label: "Graph color (hex)", required: true, pattern: "[0-9a-fA-F]{6}" }, { name: "showOnDashboard", label: "Show on dashboard", type: "checkbox" }]}
      onSave={(model) => request(editing ? `/expense-types/${parts[2]}` : "/expense-types", { method: editing ? "PUT" : "POST", body: model })} />
    : <section><div className="heading"><h1>Expense types</h1><Link className="button" to="/expense-types/create">Create</Link></div><Table rows={data} columns={[["name", "Name"], ["graphColor", "Color"], ["showOnDashboard", "Dashboard", (r) => r.showOnDashboard ? "Yes" : "No"]]} actions={(r) => <><Link to={`/expense-types/edit/${r.id}`}>Edit</Link><Action danger onClick={async () => { if (confirm(`Delete ${r.name}?`)) { await request(`/expense-types/${r.id}`, { method: "DELETE" }); setRefresh(refresh + 1); } }}>Delete</Action></>} /></section>}</Loader>;
}

function Banks({ parts }) {
  const [refresh, setRefresh] = useState(0);
  const editing = parts[1] === "edit";
  const creating = parts[1] === "create";
  const state = useData(creating || editing ? ["/countries", ...(editing ? [`/banks/${parts[2]}`] : [])] : ["/banks"], refresh);
  return <Loader state={state}>{(data) => creating || editing
    ? <BankForm key={parts.join("/")} editing={editing} countries={data[0]} initial={editing ? data[1] : { name: "", countryId: null, website: "https://", generalEnquiryPhoneNumber: "00000000000", iconPath: "" }} />
    : <section><div className="heading"><h1>Banks</h1><Link className="button" to="/banks/create">Create</Link></div><Table rows={data[0]} columns={[["name", "Name", (r) => <>{r.renderedIcon && <img className="bank-icon" src={r.renderedIcon} alt="" />} {r.name}</>], ["countryName", "Country"]]} actions={(r) => <><Link to={`/banks/edit/${r.id}`}>Edit</Link><Action danger onClick={async () => { if (confirm(`Delete ${r.name}?`)) { await request(`/banks/${r.id}`, { method: "DELETE" }); setRefresh(refresh + 1); } }}>Delete</Action></>} /></section>}</Loader>;
}

function BankForm({ editing, countries, initial }) {
  const [file, setFile] = useState(null);
  return <Form title={`${editing ? "Edit" : "Create"} bank`} initial={initial} cancel="/banks"
    fields={[{ name: "countryId", label: "Country", required: true, options: countries }, { name: "name", label: "Name", required: true, maxLength: 50 }, { name: "website", label: "Website", type: "url", required: true }, { name: "generalEnquiryPhoneNumber", label: "Phone number", required: true, pattern: "[0-9]{11}" }]}
    onSave={(model) => { const data = new FormData(); for (const key of ["name", "countryId", "website", "generalEnquiryPhoneNumber", "iconPath"]) data.append(key, model[key] ?? ""); if (file) data.append("icon", file); return request(editing ? `/banks/${model.id}` : "/banks", { method: editing ? "PUT" : "POST", body: data }); }}>
    <label className="field">Bank icon<input type="file" accept="image/*" required={!editing} onChange={(e) => setFile(e.target.files[0])} /></label>
  </Form>;
}

function Accounts({ parts }) {
  const [refresh, setRefresh] = useState(0);
  const creating = parts[1] === "create";
  const state = useData(creating ? ["/banks", "/currencies"] : ["/accounts"], refresh);
  return <Loader state={state}>{(data) => creating
    ? <Form title="Create bank account" initial={{ name: "", bankId: null, currencyId: null, isSavingAccount: false, initialBalance: 0 }} cancel="/bank-accounts"
      fields={[{ name: "name", label: "Name", required: true }, { name: "bankId", label: "Bank", options: data[0], required: true }, { name: "currencyId", label: "Currency", options: data[1], required: true }, { name: "initialBalance", label: "Initial balance", type: "number", step: "0.01" }, { name: "isSavingAccount", label: "Saving account", type: "checkbox" }]}
      onSave={(model) => request("/accounts", { method: "POST", body: model })} />
    : <section><div className="heading"><h1>Bank accounts</h1><Link className="button" to="/bank-accounts/create">Create</Link></div><Table rows={data[0]} columns={[["name", "Name"], ["bankName", "Bank"], ["currencyName", "Currency"], ["currentBalance", "Balance", (r) => r.displayedCurrentBalance || money(r.currentBalance)], ["isFavorite", "Favorite", (r) => r.isFavorite ? "Yes" : "No"]]} actions={(r) => <><Action onClick={async () => { await request(`/accounts/${r.id}/favorite`, { method: "POST" }); setRefresh(refresh + 1); }}>Favorite</Action>{r.canBeDeleted && <Action danger onClick={async () => { if (confirm(`Delete ${r.name}?`)) { await request(`/accounts/${r.id}`, { method: "DELETE" }); setRefresh(refresh + 1); } }}>Delete</Action>}</>} /></section>}</Loader>;
}

const movementConfig = {
  expenses: { label: "Expenses", slug: "expenses", date: "dateExpense", amount: "cost" },
  incomes: { label: "Incomes", slug: "incomes", date: "dateIncome", amount: "cost" },
  savings: { label: "Savings", slug: "savings", date: "dateSaving", amount: "amount" },
  withdrawals: { label: "ATM withdrawals", slug: "atm-withdraws", date: "dateExpense", amount: "initialAmount" },
  budgets: { label: "Budget plans", slug: "budget-plans" },
};

function Movements({ kind, parts }) {
  const config = movementConfig[kind];
  const accountId = Number(parts[2]);
  const base = `/${config.slug}/list/${accountId}`;
  const [refresh, setRefresh] = useState(0);
  const [page, setPage] = useState(0);
  const action = parts[1];
  const id = parts[3];
  const editing = action === "edit" || action === "view";
  const creating = action === "create";
  const paths = creating || editing
    ? ["/accounts", "/expense-types", "/payment-methods", `/accounts/${accountId}/withdrawals`, ...(editing ? [`/budgets/${id}`] : []), ...(kind === "budgets" ? [`/accounts/${accountId}/budget-context${editing ? `?excludeId=${id}` : ""}`] : [])]
    : [`/accounts/${accountId}/${kind}`];
  const state = useData(paths, refresh);
  return <Loader state={state}>{(data) => creating || editing
    ? <MovementForm kind={kind} accountId={accountId} base={base} data={data} editing={editing} view={action === "view"} />
    : <section><div className="heading"><div><Link to={`/${accountId}`}>← Dashboard</Link><h1>{config.label}</h1></div><Link className="button" to={`/${config.slug}/create/${accountId}`}>Create</Link></div>
      <Table rows={kind === "expenses" ? data[0].slice(page * 10, page * 10 + 10) : data[0]} columns={kind === "budgets" ? [["name", "Name"], ["displayedStartDate", "Start"], ["displayedEndDate", "End"]] : [[config.date, "Date", (r) => date(r[config.date])], ["description", "Description"], [config.amount, "Amount", (r) => money(r[config.amount])], ...(kind === "expenses" ? [["expenseTypeName", "Category"]] : []), ...(kind === "withdrawals" ? [["currentAmount", "Remaining", (r) => money(r.currentAmount)], ["isClosed", "Closed", (r) => r.isClosed ? "Yes" : "No"]] : []), ...(kind === "savings" ? [["targetInternalAccountName", "Target account"]] : [])]}
        actions={(r) => kind === "budgets" ? <><Link to={`/budget-plans/view/${accountId}/${r.id}`}>View</Link>{r.canBeChanged && <><Link to={`/budget-plans/edit/${accountId}/${r.id}`}>Edit</Link><Action onClick={async () => { await request(`/accounts/${accountId}/budgets/${r.id}/start`, { method: "POST" }); setRefresh(refresh + 1); }}>Start</Action></>}{r.startDate && !r.endDate && <Action onClick={async () => { await request(`/budgets/${r.id}/stop`, { method: "POST" }); setRefresh(refresh + 1); }}>Stop</Action>}</>
          : <>{(kind === "withdrawals" || (kind === "expenses" && r.paymentMethodHasBeenAlreadyDebitedOption)) && <Action onClick={async () => { await request(`/${kind}/${r.id}/debit`, { method: "POST", body: { current: r.hasBeenAlreadyDebited } }); setRefresh(refresh + 1); }}>{r.hasBeenAlreadyDebited ? "Mark not debited" : "Mark debited"}</Action>}
            {kind === "withdrawals" && !r.isClosed && <span>Closing is not available offline</span>}
            <Action danger onClick={async () => { if (confirm("Delete this item?")) { await request(`/${kind}/${r.id}`, { method: "DELETE" }); if (kind === "expenses" && page > 0 && data[0].length === page * 10 + 1) setPage(page - 1); setRefresh(refresh + 1); } }}>Delete</Action></>} />
      {kind === "expenses" && <div className="pagination"><button className="secondary" disabled={page === 0} onClick={() => setPage(page - 1)}>Previous</button><span>Page {page + 1} of {Math.max(1, Math.ceil(data[0].length / 10))}</span><button className="secondary" disabled={(page + 1) * 10 >= data[0].length} onClick={() => setPage(page + 1)}>Next</button></div>}
    </section>}</Loader>;
}

function MovementForm({ kind, accountId, base, data, editing, view }) {
  const [accounts, expenseTypes, methods, withdrawals] = data;
  const existing = editing ? data[4] : null;
  const budgetContext = kind === "budgets" ? data[editing ? 5 : 4] : null;
  const config = movementConfig[kind];
  const initial = existing || (kind === "budgets"
    ? { name: "", expectedIncomes: 0, expectedSavings: 0, expenseTypes: expenseTypes.filter((x) => x.showOnDashboard).map((expenseType) => ({ expenseType, expectedValue: 0 })) }
    : { [config.date]: today(), [config.amount]: 0, description: "", expenseTypeId: null, paymentMethodId: null, targetInternalAccountId: null, atmWithdrawId: null, hasBeenAlreadyDebited: false });
  const fields = kind === "budgets" ? [{ name: "name", label: "Name", required: true }, { name: "plannedStartDate", label: "Planned start date", type: "date" }, { name: "expectedIncomes", label: "Expected incomes", type: "number", min: 0, step: "0.01", disabled: editing }, { name: "expectedSavings", label: "Expected savings", type: "number", min: 0, step: "0.01", disabled: editing }]
    : [{ name: config.date, label: "Date", type: "date", required: true }, ...(kind === "expenses" || kind === "incomes" ? [{ name: "description", label: "Description", required: true }] : []), { name: config.amount, label: "Amount", type: "number", min: 0, step: "0.01", required: true }, ...(kind === "expenses" ? [{ name: "expenseTypeId", label: "Expense type", options: expenseTypes, required: true }, { name: "paymentMethodId", label: "Payment method", options: methods, required: true }] : []), ...(kind === "savings" ? [{ name: "targetInternalAccountId", label: "Saving account", options: accounts.filter((a) => a.isSavingAccount), required: true }] : []), ...(kind === "withdrawals" ? [{ name: "hasBeenAlreadyDebited", label: "Already debited", type: "checkbox" }] : [])];
  if (view) return <section><h1>Budget plan: {existing.name}</h1><p>Expected incomes: {money(existing.expectedIncomes)} · savings: {money(existing.expectedSavings)} · expenses: {money(existing.expectedExpenses)}</p><Table rows={existing.expenseTypes.map((e, i) => ({ id: i, name: e.expenseType.name, value: e.expectedValue }))} columns={[["name", "Expense type"], ["value", "Expected"]]} /><Link to={base}>Back</Link></section>;
  return <Form title={`${editing ? "Edit" : "Create"} ${config.label.toLowerCase()}`} initial={initial} fields={fields} cancel={base} onSave={(model) => request(`/accounts/${accountId}/${kind}${editing ? `/${model.id}` : ""}`, { method: editing ? "PUT" : "POST", body: model })}>
    {(model, setModel) => <>{kind === "expenses" && model.paymentMethodId === 2 && <Field label="ATM withdrawal" name="atmWithdrawId" model={model} setModel={setModel} options={withdrawals.filter((w) => !w.isClosed).map((w) => ({ ...w, name: w.description }))} required />}
      {kind === "expenses" && model.paymentMethodId === 5 && <Field label="Target account" name="targetInternalAccountId" model={model} setModel={setModel} options={accounts} required />}
      {kind === "expenses" && model.paymentMethodId && model.paymentMethodId !== 2 && model.paymentMethodId !== 5 && <Field label="Already debited" name="hasBeenAlreadyDebited" model={model} setModel={setModel} type="checkbox" />}
      {kind === "budgets" && <>{editing && <p>Expected income and savings cannot be changed by the existing offline service.</p>}<h2>Expense type budgets</h2><p>Compare with the previous plan, last month and six-month average.</p><div className="table-wrap"><table><thead><tr><th>Category</th><th>Expected</th><th>Previous plan</th><th>Last month</th><th>Six-month average</th></tr></thead><tbody>{model.expenseTypes.map((entry, index) => <tr key={entry.expenseType.id}><td><label htmlFor={`budget-${entry.expenseType.id}`}>{entry.expenseType.name}</label></td><td><input id={`budget-${entry.expenseType.id}`} type="number" step="0.01" min="0" value={entry.expectedValue} onChange={(e) => setModel({ ...model, expenseTypes: model.expenseTypes.map((item, i) => i === index ? { ...item, expectedValue: Number(e.target.value) } : item) })} /></td><td>{money(budgetContext.previous?.expenseTypes.find((item) => item.expenseType.id === entry.expenseType.id)?.expectedValue)}</td><td>{money(budgetContext.lastMonth.aggregates[entry.expenseType.name])}</td><td>{money(budgetContext.average.aggregates[entry.expenseType.name])}</td></tr>)}</tbody></table></div>
        <p>Total: {money(model.expectedIncomes - model.expectedSavings - model.expenseTypes.reduce((sum, item) => sum + item.expectedValue, 0))}</p></>}</>}
  </Form>;
}

const root = import.meta.hot?.data.root ?? createRoot(document.getElementById("root"));
if (import.meta.hot) import.meta.hot.data.root = root;
root.render(<App />);
