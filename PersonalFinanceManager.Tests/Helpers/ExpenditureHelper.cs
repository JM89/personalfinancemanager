using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.ExpenditureType;

namespace PersonalFinanceManager.Tests.Helpers
{
    public static class ExpenditureHelper
    {
        public static ExpenditureModel CreateExpenditureModel(int id, DateTime date, decimal cost, int typeId)
        {
            var entity = new ExpenditureModel
            {
                Id = id, 
                Description = "Expense " + id, 
                DateExpenditure = date,
                TypeExpenditureId = typeId, 
                Cost = cost
            };
            return entity;
        }
    }
}
