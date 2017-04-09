using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.ExpenditureType;

namespace PersonalFinanceManager.Tests.Helpers
{
    public static class IncomeHelper
    {
        public static IncomeModel CreateIncomeModel(int id, DateTime date, decimal cost, int accountId)
        {
            var entity = new IncomeModel
            {
                Id = id, 
                Description = "Income " + id, 
                DateIncome = date,
                Cost = cost, 
                AccountId = accountId
            };
            return entity;
        }
    }
}
