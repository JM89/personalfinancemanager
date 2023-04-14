using System.Collections.Generic;
using PFM.Services.Core;
using PFM.Services.DTOs.BudgetPlan;
using PFM.Services.DTOs.Dashboard;
using PFM.Services.DTOs.Expense;

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

        IList<ExpenseList> GetExpenses(DTOs.SearchParameters.ExpenseGetListSearchParameters search);
    }
}