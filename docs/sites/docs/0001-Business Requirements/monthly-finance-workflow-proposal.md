# Monthly finance workflow proposal

**Status: PROPOSED - not approved or implemented.**

**Assessment date: 2026-11-18.** Audience: PFM owner/product stakeholder.

## TL;DR

- Extend PFM's existing savings, tax and pension foundations into a monthly review workflow.
- Keep posted savings separate from forecasts; show destination totals and historical monthly tax/pension information together.
- This needs additional records and summary views, not just charts or a whole rewrite. Business rules below require agreement before delivery.

This proposal does not change accepted requirements or their statuses.
Related specifications: [savings (EI_07)](./EI_07.md),
[dashboard (D_02)](./D_02.md), [budget (BP_01)](./BP_01.md), and
[Tax & Pension website scope / AT_02/AT_03](./0000-overview.md#tax--pension-website-scope).

## Workflow observed in the spreadsheet

Source: local `Untitled.numbers`, outside the repository, containing fake data.
It has three sheets with one table each; it is evidence of a workflow, not an approved specification.

| Step / sheet | Fields and observed use |
| --- | --- |
| Record savings: **Savings - Details** | 14 rows with Transfer Date, Amount, Amount With Forecast, Currency, Source Account and Destination Account. Both amount columns are manually entered, not formula-derived. All sample currencies are GBP; Amount values are zero, with no established zero/missing-value meaning. |
| Review savings: **Savings - Summary** | Groups destinations Saving A/B/C, with separate sums for both amount columns and a grand total, referencing pivot-source totals. "Amount With Forecast" is not established as an extra amount or a value derived from Amount. |
| Review income/tax/pension: **Income Tax & Pension - Summary** | 12 monthly rows, April 2024 to March 2025: Date, Taxable Income, Income Tax Paid, National Insurance Paid (these three monetary fields in GBP), Salary Sacrifice (100% Pension), Pension Contribution (%), and Pension Contribution Total; financial totals and an income-threshold indicator follow. |

The tax/pension headers attribute taxable income, tax and NI to **HMRC**,
salary sacrifice to **Payslip**, contribution percentage to **HR**, and
contribution total to **Pension Scheme A**. These are source labels, not integrations.
Monthly pension contribution amounts are entered manually, not derived from salary or percentage.
The contribution-total header also says **"15th next month"**, whose meaning remains open.

## Current PFM compared with the proposed outcome

| Area | Current PFM | Proposed outcome |
| --- | --- | --- |
| Savings entries | List/create/delete, with date, amount and destination; selected account supplies the source and its currency. No edit action or update route, despite older EI_07 wording. Live saving creation affects account balances. | Maintain dated savings inputs and distinguish posted transfers from planning values without treating a forecast as money moved. Correction rules need agreement. |
| Savings review | Monthly savings for the selected account. A budget's single `expectedSavings` value is not a forecast per transfer/destination. | Separate Amount and Amount With Forecast totals by destination and reporting period; show currency-specific totals as a proposed guardrail, without implied conversion. |
| Tax review | Create/list/edit/delete date-based reports: currency, country, taxable income, tax paid, NI paid and payday. No combined April-March grid, annual totals or threshold view. | Review monthly tax values alongside pension information and agreed period totals; any threshold indicator must use an agreed, labelled rule. |
| Pension review | Scheme name/link, country/currency, current pot/contribution and last-updated snapshot; edits overwrite current values. | Preserve monthly sacrifice, percentage and contribution history associated with the relevant reporting month, rather than replacing it with the latest snapshot. |

Implementation evidence: [website workflows](../../../../PFM.Website.Reboot/src/PFM.Website.Client/src/main.jsx),
[live transfer behavior](../../../../PFM.Api/src/PFM.Services/MovementStrategy/InternalTransferMovementStrategy.cs),
[tax-report fields](../../../../PFM.TNP.Api/src/Api.Contracts/IncomeTaxReport/IncomeTaxReportSaveRequest.cs),
and [pension snapshot updates](../../../../PFM.TNP.Api/src/Services/PensionService.cs).

## Draft user stories and acceptance criteria

These are proposed observable outcomes, not claims of existing behavior.

### 1. Review savings without confusing plans with transfers

As the account owner, I want destination-based savings totals so I can review Amount and Amount With Forecast independently.

| Given | When | Then |
| --- | --- | --- |
| Two GBP rows for one destination in the selected period have Amount 100 and 200, and Amount With Forecast 150 and 250 | I view its summary | Amount totals 300 and Amount With Forecast totals 400, displayed separately without adding the two totals together. |
| I enter or change a planning value | I save it | That planning action does not debit or credit an account or create a posted transfer. |
| Destinations use different currencies | I view totals | Totals remain separated by currency; there is no unlabelled mixed-currency grand total or assumed exchange rate. |

### 2. Keep a monthly tax and pension history

As the account owner, I want a combined monthly record so I can review income, deductions and pension contributions without losing earlier months.

| Given | When | Then |
| --- | --- | --- |
| A reporting period and month-assignment rules have been agreed | I enter a month's tax and pension information | The monthly view retains the observed fields, currency/country context and source labels; manually entered values are not silently recalculated from salary or percentage. |
| An earlier month's pension information exists | I record a later month's information | Both months remain reviewable; the later entry does not replace the earlier history. |
| Monthly records exist and inclusion rules have been agreed | I review period totals | Totals follow those rules and make excluded or missing months visible rather than silently treating them as zero. |

### 3. Understand the annual review without mistaking it for tax advice

As the account owner, I want a clear period summary so I can identify information to check without relying on an unverified filing rule.

| Given | When | Then |
| --- | --- | --- |
| No threshold rule has been confirmed for my jurisdiction and reporting year | I review the summary | PFM does not present the workbook's 100,000 heuristic as a legal filing requirement or tax payable. |
| A threshold rule and its purpose have been approved | I view an indicator | Its jurisdiction, applicable period, basis and meaning are visible, distinct from any tax-payable calculation. |

## Decisions and source caveats to resolve

- **Actual versus forecast:** define both amount columns, how planning relates to a posted transfer, correction behavior, and whether zero differs from missing. Do not assume Amount With Forecast is additional to Amount.
- **Period and currency:** confirm boundaries and month assignment for savings and tax/pension reporting. An April-March grid does not establish a precise tax-year cutoff. Agree currency aggregation; currency-separated totals above are a proposed safeguard, not observed conversion.
- **Fake-source dates:** Savings - Details A12 and A13 display Jan-25 but store 25 January 2024; A14 displays Feb-25 but stores 25 February 2024. Resolve before any optional import; neither reproduce nor silently correct this ambiguity.
- **Pension timing and totals:** clarify whether "15th next month" means reference month, payment date or credited date. B-E totals sum rows 3-14, but G15 uses `SUM(G4:G14)`, excluding April's G3 contribution. Confirm whether that exclusion is intentional before defining totals.
- **Threshold intent:** B16 subtracts 100,000 from total taxable income and is labelled "Personal Tax Declaration (if positive)". This is an unconfirmed workbook heuristic, not validated law or tax payable. Confirm jurisdiction, year and intended action before adopting any indicator.

## Live, offline and scope boundaries

| Mode | Current foundation and proposed expectation |
| --- | --- |
| Live | Existing savings are balance-affecting transfers; tax/pension use authenticated services. The proposed planning and review workflow must keep forecasts separate from posted savings. Live external-service behavior was not exercised in this assessment. |
| Offline demo | Existing in-memory savings create/list/delete and tax/pension CRUD were exercised. Changes are temporary and reset when the host stops. Future approved features should expose matching observable planning, history and summary behavior with demo data; this does not imply the current demo performs live debit/credit events. |

Spreadsheet migration/import and export are **optional follow-up decisions**, not included commitments.
This proposal does not choose a storage design or introduce tax advice, automatic payroll/pension derivation or source-system integrations.

**Validation boundary:** the existing offline test passed (1 passed, 0 failed/skipped).
It validates existing workflows, not spreadsheet parity or the draft criteria above.
Live external services and all newly proposed behavior remain unvalidated.
