using System.Collections.Generic;
using PFM.Api.Contracts.ExpenseType;

namespace PFM.Services.Interfaces
{
    public interface IExpenseTypeService : IBaseService
    {
        IList<ExpenseTypeList> GetExpenseTypes();

        ExpenseTypeDetails GetById(int id);

        void CreateExpenseType(ExpenseTypeDetails expenditureTypeDetails);

        void EditExpenseType(ExpenseTypeDetails expenditureTypeDetails);

        void DeleteExpenseType(int id);
    }
}