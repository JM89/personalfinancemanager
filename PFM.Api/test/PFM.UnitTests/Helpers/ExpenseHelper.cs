using PFM.DataAccessLayer.Entities;
using System;

namespace PFM.UnitTests.Helpers
{
    public static class ExpenseHelper
    {
        public static Expense CreateExpenseModel(int id, DateTime date, decimal cost, int typeId)
        {
            var entity = new Expense
            {
                Id = id, 
                Description = "Expense " + id, 
                DateExpense = date,
                ExpenseTypeId = typeId, 
                Cost = cost
            };
            return entity;
        }
    }
}
