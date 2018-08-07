using System.Collections.Generic;
using PFM.Services.Core;
using PFM.DTOs.ExpenseType;

namespace PFM.Services.Interfaces
{
    public interface IExpenseTypeService : IBaseService
    {
        IList<ExpenseTypeList> GetExpenditureTypes();

        ExpenseTypeDetails GetById(int id);

        void CreateExpenditureType(ExpenseTypeDetails expenditureTypeDetails);

        void EditExpenditureType(ExpenseTypeDetails expenditureTypeDetails);

        void DeleteExpenditureType(int id);
    }
}