using PFM.Api.Contracts.Expense;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PFM.Services.Interfaces
{
    public interface IExpenseService : IBaseService
    {
        Task<bool> CreateExpenses(List<ExpenseDetails> ExpenseDetails);

        Task<bool> CreateExpense(ExpenseDetails ExpenseDetails);

        Task<bool> DeleteExpense(int id);

        Task<ExpenseDetails> GetById(int id);

        Task<bool> ChangeDebitStatus(int id, bool debitStatus);

        Task<IList<ExpenseList>> GetExpenses(Api.Contracts.SearchParameters.ExpenseGetListSearchParameters search);
    }
}