using PFM.DataAccessLayer.Entities;
using System;

namespace PFM.UnitTests.Helpers
{
    public static class IncomeHelper
    {
        public static Income CreateIncomeModel(int id, DateTime date, decimal cost, int accountId)
        {
            var entity = new Income
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
