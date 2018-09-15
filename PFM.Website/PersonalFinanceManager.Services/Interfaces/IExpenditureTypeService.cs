using System.Collections.Generic;
using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Services.Core;

namespace PersonalFinanceManager.Services.Interfaces
{
    public interface IExpenditureTypeService : IBaseService
    {
        IList<ExpenditureTypeListModel> GetExpenditureTypes();

        ExpenditureTypeEditModel GetById(int id);

        void CreateExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel);

        void EditExpenditureType(ExpenditureTypeEditModel expenditureTypeEditModel);

        void DeleteExpenditureType(int id);
    }
}