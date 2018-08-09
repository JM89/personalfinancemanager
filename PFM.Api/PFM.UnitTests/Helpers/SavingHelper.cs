using PFM.DataAccessLayer.Entities;
using System;

namespace PFM.UnitTests.Helpers
{
    public static class SavingHelper
    {
        public static Saving CreateSavingModel(int id, DateTime date, decimal cost, int accountId)
        {
            var entity = new Saving
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
