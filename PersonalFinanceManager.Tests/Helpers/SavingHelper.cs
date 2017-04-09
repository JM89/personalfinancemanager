using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Models.ExpenditureType;

namespace PersonalFinanceManager.Tests.Helpers
{
    public static class SavingHelper
    {
        public static SavingModel CreateSavingModel(int id, DateTime date, decimal cost, int accountId)
        {
            var entity = new SavingModel
            {
                Id = id, 
                Amount = cost,
                DateSaving = date, 
                AccountId = accountId
            };
            return entity;
        }
    }
}
