using System.Collections.Generic;
using PFM.Services.Core;
using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.Dashboard;
using PFM.Api.Contracts.Expense;

namespace PFM.Services.Interfaces
{
    public interface IExpenseService : IBaseService
    {
        void CreateExpenses(List<ExpenseDetails> ExpenseDetails);

        void CreateExpense(ExpenseDetails ExpenseDetails);

        void EditExpense(ExpenseDetails ExpenseDetails);

        void DeleteExpense(int id);

        ExpenseDetails GetById(int id);

        void ChangeDebitStatus(int id, bool debitStatus);

        ExpenseSummary GetExpenseSummary(int accountId, BudgetPlanDetails budgetPlan);

        IList<ExpenseList> GetExpenses(Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search);
    }
}