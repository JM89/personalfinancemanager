using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Services.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IExpenditureTypeService : IBaseService
    {
        Task<IList<ExpenditureTypeListModel>> GetExpenditureTypes();

        Task<ExpenditureTypeEditModel> GetById(int id);

        Task<bool> CreateExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel);

        Task<bool> EditExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel);

        Task<bool> DeleteExpenditureType(int id);
    }
}