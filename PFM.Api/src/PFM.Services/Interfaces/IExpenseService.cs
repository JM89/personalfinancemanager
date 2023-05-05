using PFM.Api.Contracts.BudgetPlan;
using PFM.Api.Contracts.Dashboard;
using PFM.Api.Contracts.Expense;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IExpenseService : IBaseService
    {
        Task<bool> CreateExpenses(List<ExpenseDetails> ExpenseDetails);

        Task<bool> CreateExpense(ExpenseDetails ExpenseDetails);

        Task<bool> DeleteExpense(int id);

        ExpenseDetails GetById(int id);

        void ChangeDebitStatus(int id, bool debitStatus);

        Task<ExpenseSummary> GetExpenseSummary(int accountId, BudgetPlanDetails budgetPlan, DateTime referenceDate);

        Task<IList<ExpenseList>> GetExpenses(Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search);
    }
}