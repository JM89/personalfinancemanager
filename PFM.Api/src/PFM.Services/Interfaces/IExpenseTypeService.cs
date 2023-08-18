using System.Collections.Generic;
using System.Threading.Tasks;
using PFM.Api.Contracts.ExpenseType;

namespace PFM.Services.Interfaces
{
    public interface IExpenseTypeService : IBaseService
    {
        Task<IList<ExpenseTypeList>> GetExpenseTypes(string userId);

        Task<ExpenseTypeDetails> GetById(int id);

        Task<bool> CreateExpenseType(ExpenseTypeDetails expenditureTypeDetails, string userId);

        Task<bool> EditExpenseType(ExpenseTypeDetails expenditureTypeDetails, string userId);

        Task<bool> DeleteExpenseType(int id);
    }
}