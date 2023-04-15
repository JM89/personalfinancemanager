using PFM.DataAccessLayer.Entities;
using PFM.Api.Contracts.ExpenseType;

namespace PFM.UnitTests.Helpers
{
    public static class ExpenseTypeHelper
    {
        public static ExpenseTypeList CreateExpenseTypeListModel(int id, string name)
        {
            var model = new ExpenseTypeList
            {
                Id = id,
                Name = name
            };
            return model;
        }

        public static ExpenseType CreateExpenseTypeModel(int id, string name)
        {
            var entity = new ExpenseType
            {
                Id = id,
                Name = name
            };
            return entity;
        }
    }
}
