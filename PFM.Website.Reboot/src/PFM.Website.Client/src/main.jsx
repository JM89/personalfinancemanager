import React, { useEffect, useState } from "react";
import { createRoot } from "react-dom/client";
import { request } from "./api";
import { CategoryChart, CategoryTrendChart, ExpenseTrendChart, MovementChart } from "./DashboardCharts.jsx";
import { Appearance, BrandMark, Dialog, Icon, ThemeProvider } from "./Interface.jsx";
import "./style.css";

const today = () => new Date().toISOString().slice(0, 10);
const money = (value) => Number(value || 0).toFixed(2);
const date = (value) => value ? value.slice(0, 10) : "";
const summaryLabels = {
  totalExpensesCurrentMonth: ["Spent this month", "Current calendar month"],
  averageExpensesOver12Months: ["Monthly expenses", "12-month average"],
  averageIncomesOver12Months: ["Monthly income", "12-month average"],
  averageSavingsOver12Months: ["Monthly savings", "12-month average"],
};

function Link({ to, children, onClick, ...props }) {
  return <a href={to} {...props} onClick={(event) => {
    onClick?.(event);
    if (event.defaultPrevented) return;
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
  if (state.loading) return <div className="loading-state" role="status"><span className="loading-dot" />Getting things ready...</div>;
  if (state.error) return <p className="error error-panel" role="alert">{state.error}</p>;
  return children(state.data);
}

function Action({ children, onClick, danger = false, confirmation, icon, label }) {
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  const [open, setOpen] = useState(false);
  const execute = async () => {
    setError("");
    setBusy(true);
    try { await onClick(); setOpen(false); } catch (e) { setError(e.message); } finally { setBusy(false); }
  };
  return <><button type="button" className={`${danger ? "danger" : "secondary"}${icon ? " operation-icon" : ""}`} aria-label={icon ? busy ? `Working: ${label}` : label : undefined} title={icon ? label : undefined} aria-busy={busy || undefined} disabled={busy} onClick={() => confirmation ? setOpen(true) : execute()}>{icon ? <Icon name={icon} /> : busy ? "Working..." : children}</button>
    {error && !open && <span className="error" role="alert">{error}</span>}
    {open && <Dialog title={danger ? "Delete this item?" : "Confirm this action"} onClose={() => { if (!busy) { setOpen(false); setError(""); } }}>
      <div className="confirmation-icon"><Icon name="category" /></div><p>{confirmation}</p>{danger && <p className="muted">This cannot be undone. Everything else will stay as it is.</p>}
      {error && <p className="error" role="alert">{error}</p>}
      <div className="actions"><button type="button" className="secondary" disabled={busy} data-dialog-autofocus onClick={() => { setOpen(false); setError(""); }}>{danger ? "Keep item" : "Cancel"}</button><button type="button" className={danger ? "danger solid" : undefined} disabled={busy} onClick={execute}>{busy ? "Working..." : danger ? "Yes, delete" : children}</button></div>
    </Dialog>}
  </>;
}

function EditOperation({ to, name }) {
  return <Link className="operation-icon secondary" to={to} aria-label={`Edit ${name}`} title={`Edit ${name}`}><Icon name="edit" /></Link>;
}

function Field({ label, name, model, setModel, type = "text", required = false, options, hint, ...props }) {
  const value = model[name] ?? (type === "checkbox" ? false : "");
  const update = (event) => setModel({ ...model, [name]: type === "checkbox" ? event.target.checked : type === "number" || options ? (event.target.value === "" ? null : Number(event.target.value)) : type === "color" ? event.target.value.slice(1).toUpperCase() : type === "date" && event.target.value === "" ? null : event.target.value });
  return <label className={`field ${type === "checkbox" ? "checkbox-field" : ""}`}><span className="field-label">{label}{required && <span className="required-mark" aria-hidden="true"> *</span>}</span>
    {options ? <select {...props} value={value} onChange={update} required={required}><option value="">Choose {label.toLowerCase()}...</option>{options.map((item) => <option key={item.id} value={item.id}>{item.name}</option>)}</select>
      : <input {...props} type={type} value={type === "checkbox" ? undefined : type === "color" ? `#${value}` : type === "date" ? date(value) : value} checked={type === "checkbox" ? value : undefined} onChange={update} required={required} />}
    {hint && <small className="field-hint">{hint}</small>}
  </label>;
}

function Form({ title, initial, fields, onSave, cancel, children }) {
  const [model, setModel] = useState(initial);
  const [error, setError] = useState("");
  const [busy, setBusy] = useState(false);
  return <section className="form-page"><Link className="back-link" to={cancel}>← Back to overview</Link><div className="page-intro"><span className="eyebrow">YOUR WORKSPACE, YOUR WAY</span><h1>{title}</h1><p className="muted">A few details now, a clearer picture of your finances later.</p></div><form className="card form" onSubmit={async (event) => {
    event.preventDefault();
    setError("");
    setBusy(true);
    try { await onSave(model); navigate(cancel); } catch (e) { setError(e.message); } finally { setBusy(false); }
  }}>
    <div className="form-section-heading"><span className="step-marker">1</span><div><h2>The essentials</h2><p className="muted">Fields marked * are required.</p></div></div>
    <fieldset disabled={busy} className="form-fields">{fields.map((field) => <Field key={field.name} {...field} model={model} setModel={setModel} />)}
    {typeof children === "function" ? children(model, setModel) : children}
    </fieldset>
    {error && <p className="error" role="alert">{error}</p>}
    <div className="actions form-footer"><span className="muted">Review your details before saving.</span><Link to={cancel} aria-disabled={busy} onClick={(event) => { if (busy) event.preventDefault(); }}>Cancel</Link><button disabled={busy}>{busy ? "Saving..." : "Save changes"}</button></div>
  </form></section>;
}

function Table({ columns, rows, actions }) {
  return rows.length === 0 ? <div className="empty-state"><Icon name="wallet" width="32" height="32" /><h2>A clean slate</h2><p className="muted">Nothing here yet. Create your first item to get started.</p></div> : <div className="table-wrap"><table><thead><tr>{columns.map(([key, title]) => <th key={key} scope="col">{title}</th>)}{actions && <th scope="col">Actions</th>}</tr></thead><tbody>
    {rows.map((row) => <tr key={row.id}>{columns.map(([key, , render]) => <td key={key}>{render ? render(row) : String(row[key] ?? "")}</td>)}{actions && <td className="row-actions"><div className="row-action-items">{actions(row)}</div></td>}</tr>)}
  </tbody></table></div>;
}

function App() {
  const path = usePath();
  const session = useData(["/session"]);
  const mode = useData(["/mode"]);
  const offline = mode.data?.[0].offline ?? false;
  if (session.loading || mode.loading) return <main><p>Loading...</p></main>;
  if (session.error) return session.status === 401
    ? <main className="login card"><BrandMark /><span className="eyebrow">A LITTLE CLARITY GOES A LONG WAY</span><h1>Your money.<br />A clearer picture.</h1><p className="muted">Welcome to Personal Finance Manager. Your space to spend thoughtfully, plan ahead, and see the bigger picture.</p><a className="button" href={`/login?redirectUri=${encodeURIComponent(path)}`}>{offline ? "Enter your workspace" : "Sign in"} <Icon name="arrow" /></a>{offline && <p className="form-note"><Icon name="info" /> Offline development · Temporary local data</p>}<Appearance /></main>
    : <main><p className="error" role="alert">Could not connect to the website: {session.error}</p></main>;
  const navigation = [["/", "overview", "Overview"], ["/bank-accounts", "wallet", "Bank accounts"], ["/banks", "bank", "Banks"], ["/expense-types", "category", "Expense types"]];
  const active = navigation.find(([to]) => to !== "/" && path.startsWith(to))?.[0] || "/";
  return <div className="layout"><aside className="sidebar"><Link className="brand" to="/"><BrandMark /><span>PFM<span className="brand-subtitle">PERSONAL FINANCE</span></span></Link><span className="nav-label">WORKSPACE</span><nav aria-label="Main navigation">
    {navigation.map(([to, icon, label]) => <Link key={to} to={to} className={active === to ? "active" : ""} aria-current={active === to ? "page" : undefined}><Icon name={icon} />{label}{active === to && <span className="active-dot" />}</Link>)}
  </nav><div className="sidebar-bottom">{offline && <div className="workspace-note"><span className="status-dot" />Offline workspace<p>A little room to explore.<br />Data resets when the server stops.</p></div>}<Appearance /><a className="logout-link" href="/logout"><Icon name="logout" />Log out</a></div></aside><main><header className="topbar"><span className="breadcrumb">Workspace <span>/</span> <strong>{navigation.find(([to]) => to === active)?.[2]}</strong></span><div className="user-menu">{offline && <span className="offline-badge"><span className="status-dot" />Local mode</span>}<span className="avatar">{session.data[0].name?.slice(0, 1).toUpperCase() || "U"}</span><span>{session.data[0].name}</span></div></header>
    <Page key={path} path={path} offline={offline} />
  </main></div>;
}

function Page({ path, offline }) {
  const parts = path.split("/").filter(Boolean);
  if (!parts.length || (parts.length === 1 && /^\d+$/.test(parts[0]))) return <Dashboard selected={parts[0]} />;
  if (parts[0] === "expense-types") return <div className="configuration-page"><ExpenseTypes parts={parts} /></div>;
  if (parts[0] === "banks") return <div className="configuration-page"><Banks parts={parts} /></div>;
  if (parts[0] === "bank-accounts") return <div className="configuration-page"><Accounts parts={parts} /></div>;
  const kinds = { expenses: "expenses", incomes: "incomes", savings: "savings", "atm-withdraws": "withdrawals", "budget-plans": "budgets" };
  if (Object.hasOwn(kinds, parts[0]) && parts[2]) return <Movements key={path} kind={kinds[parts[0]]} parts={parts} offline={offline} />;
  return <section><h1>Page not found</h1><Link to="/">Go to dashboard</Link></section>;
}

function Dashboard({ selected }) {
  const accounts = useData(["/accounts"]);
  return <Loader state={accounts}>{([list]) => {
    const current = list.find((a) => String(a.id) === selected) || list.find((a) => a.isFavorite && !a.isSavingAccount) || list.find((a) => !a.isSavingAccount);
    return <section><div className="heading dashboard-heading"><div><span className="eyebrow">THE BIG PICTURE</span><h1>Your money, at a glance<span className="accent">.</span></h1><p className="muted">Less guesswork. More room for what matters.</p></div><Link className="button secondary" to="/bank-accounts/create"><Icon name="plus" />Add account</Link></div>
      {current ? <><div className="account-overview"><div className="balance-panel"><div className="balance-heading"><span><Icon name="wallet" />Account balance</span><span className="balance-tag">{current.isSavingAccount ? "Savings" : "Current account"}</span></div>
        <strong className="balance-amount">{current.displayedCurrentBalance || money(current.currentBalance)}</strong>
        <div className="balance-footer"><span>{current.bankName || "Your personal account"}</span><Icon name="trend" width="38" height="38" /></div></div>
        <div className="account-tools"><span className="eyebrow">YOUR ACCOUNTS</span><h2>One place. A clearer view.</h2><p className="muted">Switch accounts to explore your spending, savings and plans.</p><label className="account-select">Selected account<select value={current.id} onChange={(e) => navigate(`/${e.target.value}`)}>{list.map((a) => <option key={a.id} value={a.id}>{a.name}</option>)}</select></label></div></div>
        <nav className="movement-links" aria-label="Account activity">{[["Expenses", "expenses", "category"], ["ATM withdrawals", "atm-withdraws", "wallet"], ["Incomes", "incomes", "trend"], ["Savings", "savings", "spark"], ["Budget plans", "budget-plans", "overview"]].map(([label, slug, icon]) => <Link key={slug} to={`/${slug}/list/${current.id}`}><Icon name={icon} /><span>{label}</span><Icon name="arrow" width="16" height="16" /></Link>)}</nav>
        <DashboardData accountId={current.id} /></>
        : <div className="empty-state"><Icon name="wallet" /><h2>Your next chapter starts here</h2><p>Add a <Link to="/banks/create">bank</Link>, then a <Link to="/bank-accounts/create">bank account</Link> to see your finances come together.</p></div>}
    </section>;
  }}</Loader>;
}

function DashboardData({ accountId }) {
  const state = useData([`/accounts/${accountId}/dashboard`, "/expense-types"]);
  const [category, setCategory] = useState(null);
  return <Loader state={state}>{([data, expenseTypes]) => <><div className="stats">
    {Object.entries(data.summary).map(([name, value], index) => <div className={`card stat-card stat-${index % 4}`} key={name}><span className="stat-label">{summaryLabels[name]?.[0] || name.replace(/([A-Z])/g, " $1")}<Icon name={/income/i.test(name) ? "trend" : /saving/i.test(name) ? "spark" : "wallet"} /></span><strong>{money(value)}</strong><span className="stat-caption">{summaryLabels[name]?.[1] || "Account summary"}</span></div>)}
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
      fields={[{ name: "name", label: "Name", required: true, maxLength: 50, placeholder: "e.g. Coffee & dining" }, { name: "graphColor", label: "Chart color", type: "color", required: true, hint: "Pick a color to recognise this category in your charts." }, { name: "showOnDashboard", label: "Show on dashboard", type: "checkbox", hint: "Include this category in your financial overview." }]}
      onSave={(model) => request(editing ? `/expense-types/${parts[2]}` : "/expense-types", { method: editing ? "PUT" : "POST", body: model })} />
    : <section><div className="heading"><div><span className="eyebrow">ORGANISE YOUR EVERYDAY</span><h1>Expense types</h1><p className="muted">Give every expense a place in the bigger picture.</p></div><Link className="button" to="/expense-types/create"><Icon name="plus" />Add expense type</Link></div><Table rows={data} columns={[["name", "Name"], ["graphColor", "Color", (r) => <span className="color-label"><span className="chart-swatch" style={{ backgroundColor: /^[0-9a-f]{6}$/i.test(r.graphColor) ? `#${r.graphColor}` : undefined }} />{r.graphColor}</span>], ["showOnDashboard", "Dashboard", (r) => <span className={`badge ${r.showOnDashboard ? "positive" : ""}`}>{r.showOnDashboard ? "Visible" : "Hidden"}</span>]]} actions={(r) => <><EditOperation to={`/expense-types/edit/${r.id}`} name={r.name} /><Action danger icon="delete" label={`Delete ${r.name}`} confirmation={`“${r.name}” will be removed from your expense types.`} onClick={async () => { await request(`/expense-types/${r.id}`, { method: "DELETE" }); setRefresh(refresh + 1); }}>Delete</Action></>} /></section>}</Loader>;
}

function Banks({ parts }) {
  const [refresh, setRefresh] = useState(0);
  const editing = parts[1] === "edit";
  const creating = parts[1] === "create";
  const state = useData(creating || editing ? ["/countries", ...(editing ? [`/banks/${parts[2]}`] : [])] : ["/banks"], refresh);
  return <Loader state={state}>{(data) => creating || editing
    ? <BankForm key={parts.join("/")} editing={editing} countries={data[0]} initial={editing ? data[1] : { name: "", countryId: null, website: "https://", generalEnquiryPhoneNumber: "00000000000", iconPath: "" }} />
    : <section><div className="heading"><div><span className="eyebrow">A HOME FOR YOUR ACCOUNTS</span><h1>Banks</h1><p className="muted">Keep your banking details together, simply.</p></div><Link className="button" to="/banks/create"><Icon name="plus" />Add bank</Link></div><Table rows={data[0]} columns={[["name", "Name", (r) => <>{r.renderedIcon && <img className="bank-icon" src={r.renderedIcon} alt="" />} {r.name}</>], ["countryName", "Country"]]} actions={(r) => <><EditOperation to={`/banks/edit/${r.id}`} name={r.name} /><Action danger icon="delete" label={`Delete ${r.name}`} confirmation={`“${r.name}” will be removed from your banks.`} onClick={async () => { await request(`/banks/${r.id}`, { method: "DELETE" }); setRefresh(refresh + 1); }}>Delete</Action></>} /></section>}</Loader>;
}

function BankForm({ editing, countries, initial }) {
  const [file, setFile] = useState(null);
  return <Form title={`${editing ? "Edit" : "Create"} bank`} initial={initial} cancel="/banks"
    fields={[{ name: "countryId", label: "Country", required: true, options: countries }, { name: "name", label: "Name", required: true, maxLength: 50, placeholder: "e.g. My high street bank" }, { name: "website", label: "Website", type: "url", required: true, hint: "The bank’s full website address, starting with https://." }, { name: "generalEnquiryPhoneNumber", label: "Phone number", type: "tel", required: true, pattern: "[0-9]{11}", hint: "Use 11 digits, without spaces or a country prefix." }]}
    onSave={(model) => { const data = new FormData(); for (const key of ["name", "countryId", "website", "generalEnquiryPhoneNumber", "iconPath"]) data.append(key, model[key] ?? ""); if (file) data.append("icon", file); return request(editing ? `/banks/${model.id}` : "/banks", { method: editing ? "PUT" : "POST", body: data }); }}>
    <label className="field upload-field"><span className="field-label">Bank icon{!editing && <span className="required-mark"> *</span>}</span><input type="file" accept="image/*" required={!editing} onChange={(e) => setFile(e.target.files[0])} /><small className="field-hint">{editing ? "Choose an image to replace the current icon, or keep it as it is." : "Choose an image to make your bank easy to spot."}</small></label>
  </Form>;
}

function Accounts({ parts }) {
  const [refresh, setRefresh] = useState(0);
  const creating = parts[1] === "create";
  const state = useData(creating ? ["/banks", "/currencies"] : ["/accounts"], refresh);
  return <Loader state={state}>{(data) => creating
    ? <Form title="Create bank account" initial={{ name: "", bankId: null, currencyId: null, isSavingAccount: false, initialBalance: 0 }} cancel="/bank-accounts"
      fields={[{ name: "name", label: "Name", required: true, placeholder: "e.g. Everyday spending" }, { name: "bankId", label: "Bank", options: data[0], required: true }, { name: "currencyId", label: "Currency", options: data[1], required: true, hint: "Balances and activity use this account’s currency." }, { name: "initialBalance", label: "Initial balance", type: "number", step: "0.01", hint: "Your opening balance before adding any activity." }, { name: "isSavingAccount", label: "Saving account", type: "checkbox", hint: "Make this account available as a savings destination." }]}
      onSave={(model) => request("/accounts", { method: "POST", body: model })} />
    : <section><div className="heading"><div><span className="eyebrow">EVERY ACCOUNT, IN VIEW</span><h1>Bank accounts</h1><p className="muted">Your everyday essentials and tomorrow’s possibilities.</p></div><Link className="button" to="/bank-accounts/create"><Icon name="plus" />Add account</Link></div><Table rows={data[0]} columns={[["name", "Name", (r) => <Link className="account-name" to={`/${r.id}`}><span className="account-icon"><Icon name="wallet" /></span>{r.name}</Link>], ["bankName", "Bank"], ["currencyName", "Currency"], ["currentBalance", "Balance", (r) => r.displayedCurrentBalance || money(r.currentBalance)], ["isFavorite", "Favorite", (r) => r.isFavorite ? <span className="badge positive">Favorite</span> : "—"]]} actions={(r) => <><Action icon="star" label={`Set ${r.name} as favorite`} onClick={async () => { await request(`/accounts/${r.id}/favorite`, { method: "POST" }); setRefresh(refresh + 1); }}>Favorite</Action>{r.canBeDeleted && <Action danger icon="delete" label={`Delete ${r.name}`} confirmation={`“${r.name}” will be removed from your accounts.`} onClick={async () => { await request(`/accounts/${r.id}`, { method: "DELETE" }); setRefresh(refresh + 1); }}>Delete</Action>}</>} /></section>}</Loader>;
}

const movementConfig = {
  expenses: { label: "Expenses", slug: "expenses", date: "dateExpense", amount: "cost" },
  incomes: { label: "Incomes", slug: "incomes", date: "dateIncome", amount: "cost" },
  savings: { label: "Savings", slug: "savings", date: "dateSaving", amount: "amount" },
  withdrawals: { label: "ATM withdrawals", slug: "atm-withdraws", date: "dateExpense", amount: "initialAmount" },
  budgets: { label: "Budget plans", slug: "budget-plans" },
};

function Movements({ kind, parts, offline }) {
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
    ? <MovementForm kind={kind} accountId={accountId} base={base} data={data} editing={editing} view={action === "view"} offline={offline} />
    : <section><div className="heading"><div><Link to={`/${accountId}`}>← Dashboard</Link><h1>{config.label}</h1></div><Link className="button" to={`/${config.slug}/create/${accountId}`}>Create</Link></div>
      <Table rows={kind === "expenses" ? data[0].slice(page * 10, page * 10 + 10) : data[0]} columns={kind === "budgets" ? [["name", "Name"], ["displayedStartDate", "Start"], ["displayedEndDate", "End"]] : [[config.date, "Date", (r) => date(r[config.date])], ["description", "Description"], [config.amount, "Amount", (r) => money(r[config.amount])], ...(kind === "expenses" ? [["expenseTypeName", "Category"]] : []), ...(kind === "withdrawals" ? [["currentAmount", "Remaining", (r) => money(r.currentAmount)], ["isClosed", "Closed", (r) => r.isClosed ? "Yes" : "No"]] : []), ...(kind === "savings" ? [["targetInternalAccountName", "Target account"]] : [])]}
        actions={(r) => kind === "budgets" ? <><Link to={`/budget-plans/view/${accountId}/${r.id}`}>View</Link>{r.canBeChanged && <><Link to={`/budget-plans/edit/${accountId}/${r.id}`}>Edit</Link><Action onClick={async () => { await request(`/accounts/${accountId}/budgets/${r.id}/start`, { method: "POST" }); setRefresh(refresh + 1); }}>Start</Action></>}{r.startDate && !r.endDate && <Action onClick={async () => { await request(`/budgets/${r.id}/stop`, { method: "POST" }); setRefresh(refresh + 1); }}>Stop</Action>}</>
          : <>{(kind === "withdrawals" || (kind === "expenses" && r.paymentMethodHasBeenAlreadyDebitedOption)) && <Action onClick={async () => { await request(`/${kind}/${r.id}/debit`, { method: "POST", body: { current: r.hasBeenAlreadyDebited } }); setRefresh(refresh + 1); }}>{r.hasBeenAlreadyDebited ? "Mark not debited" : "Mark debited"}</Action>}
            {kind === "withdrawals" && !r.isClosed && (offline ? <span>Closing is not available offline</span>
              : <Action confirmation="Close this ATM withdrawal?" onClick={async () => { await request(`/withdrawals/${r.id}/close`, { method: "POST" }); setRefresh(refresh + 1); }}>Close</Action>)}
            <Action danger confirmation={`This item${r.description ? ` (${r.description})` : ""} will be removed from ${config.label.toLowerCase()}.`} onClick={async () => { await request(`/${kind}/${r.id}`, { method: "DELETE" }); if (kind === "expenses" && page > 0 && data[0].length === page * 10 + 1) setPage(page - 1); setRefresh(refresh + 1); }}>Delete</Action></>} />
      {kind === "expenses" && <div className="pagination"><button className="secondary" disabled={page === 0} onClick={() => setPage(page - 1)}>Previous</button><span>Page {page + 1} of {Math.max(1, Math.ceil(data[0].length / 10))}</span><button className="secondary" disabled={(page + 1) * 10 >= data[0].length} onClick={() => setPage(page + 1)}>Next</button></div>}
    </section>}</Loader>;
}

function MovementForm({ kind, accountId, base, data, editing, view, offline }) {
  const [accounts, expenseTypes, methods, withdrawals] = data;
  const existing = editing ? data[4] : null;
  const budgetContext = kind === "budgets" ? data[editing ? 5 : 4] : null;
  const config = movementConfig[kind];
  const initial = existing || (kind === "budgets"
    ? { name: "", expectedIncomes: 0, expectedSavings: 0, expenseTypes: expenseTypes.filter((x) => x.showOnDashboard).map((expenseType) => ({ expenseType, expectedValue: 0 })) }
    : { [config.date]: today(), [config.amount]: 0, description: "", expenseTypeId: null, paymentMethodId: null, targetInternalAccountId: null, atmWithdrawId: null, hasBeenAlreadyDebited: false });
  const amountsReadOnly = editing && offline;
  const fields = kind === "budgets" ? [{ name: "name", label: "Name", required: true }, { name: "plannedStartDate", label: "Planned start date", type: "date" }, { name: "expectedIncomes", label: "Expected incomes", type: "number", min: 0, step: "0.01", disabled: amountsReadOnly }, { name: "expectedSavings", label: "Expected savings", type: "number", min: 0, step: "0.01", disabled: amountsReadOnly }]
    : [{ name: config.date, label: "Date", type: "date", required: true }, ...(kind === "expenses" || kind === "incomes" ? [{ name: "description", label: "Description", required: true }] : []), { name: config.amount, label: "Amount", type: "number", min: 0, step: "0.01", required: true }, ...(kind === "expenses" ? [{ name: "expenseTypeId", label: "Expense type", options: expenseTypes, required: true }, { name: "paymentMethodId", label: "Payment method", options: methods, required: true }] : []), ...(kind === "savings" ? [{ name: "targetInternalAccountId", label: "Saving account", options: accounts.filter((a) => a.isSavingAccount), required: true }] : []), ...(kind === "withdrawals" ? [{ name: "hasBeenAlreadyDebited", label: "Already debited", type: "checkbox" }] : [])];
  if (view) return <section><h1>Budget plan: {existing.name}</h1><p>Expected incomes: {money(existing.expectedIncomes)} · savings: {money(existing.expectedSavings)} · expenses: {money(existing.expectedExpenses)}</p><Table rows={existing.expenseTypes.map((e, i) => ({ id: i, name: e.expenseType.name, value: e.expectedValue }))} columns={[["name", "Expense type"], ["value", "Expected"]]} /><Link to={base}>Back</Link></section>;
  return <Form title={`${editing ? "Edit" : "Create"} ${config.label.toLowerCase()}`} initial={initial} fields={fields} cancel={base} onSave={(model) => request(`/accounts/${accountId}/${kind}${editing ? `/${model.id}` : ""}`, { method: editing ? "PUT" : "POST", body: model })}>
    {(model, setModel) => <>{kind === "expenses" && model.paymentMethodId === 2 && <Field label="ATM withdrawal" name="atmWithdrawId" model={model} setModel={setModel} options={withdrawals.filter((w) => !w.isClosed).map((w) => ({ ...w, name: w.description }))} required />}
      {kind === "expenses" && model.paymentMethodId === 5 && <Field label="Target account" name="targetInternalAccountId" model={model} setModel={setModel} options={accounts} required />}
      {kind === "expenses" && model.paymentMethodId && model.paymentMethodId !== 2 && model.paymentMethodId !== 5 && <Field label="Already debited" name="hasBeenAlreadyDebited" model={model} setModel={setModel} type="checkbox" />}
      {kind === "budgets" && <>{amountsReadOnly && <p>Expected income and savings cannot be changed offline.</p>}<h2>Expense type budgets</h2><p>Compare with the previous plan, last month and six-month average.</p><div className="table-wrap"><table><thead><tr><th>Category</th><th>Expected</th><th>Previous plan</th><th>Last month</th><th>Six-month average</th></tr></thead><tbody>{model.expenseTypes.map((entry, index) => <tr key={entry.expenseType.id}><td><label htmlFor={`budget-${entry.expenseType.id}`}>{entry.expenseType.name}</label></td><td><input id={`budget-${entry.expenseType.id}`} type="number" step="0.01" min="0" value={entry.expectedValue} onChange={(e) => setModel({ ...model, expenseTypes: model.expenseTypes.map((item, i) => i === index ? { ...item, expectedValue: Number(e.target.value) } : item) })} /></td><td>{money(budgetContext.previous?.expenseTypes.find((item) => item.expenseType.id === entry.expenseType.id)?.expectedValue)}</td><td>{money(budgetContext.lastMonth.aggregates[entry.expenseType.name])}</td><td>{money(budgetContext.average.aggregates[entry.expenseType.name])}</td></tr>)}</tbody></table></div>
        <p>Total: {money(model.expectedIncomes - model.expectedSavings - model.expenseTypes.reduce((sum, item) => sum + item.expectedValue, 0))}</p></>}</>}
  </Form>;
}

const root = import.meta.hot?.data.root ?? createRoot(document.getElementById("root"));
if (import.meta.hot) import.meta.hot.data.root = root;
root.render(<ThemeProvider><App /></ThemeProvider>);
