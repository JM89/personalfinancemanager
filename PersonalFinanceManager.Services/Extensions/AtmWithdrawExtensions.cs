using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.ExpenditureStrategy;
using System.Data.Entity;

namespace PersonalFinanceManager.Services.Extensions
{
    public static class AtmWithdrawExtensions
    {
        public static void Debit(this AtmWithdrawModel atmWithdraw, ApplicationDbContext dbContext, decimal cost)
        {
            atmWithdraw.CurrentAmount -= cost;
            dbContext.Entry(atmWithdraw).State = EntityState.Modified;
            HistoricMovementHelper.SaveDebitMovement(dbContext, atmWithdraw.Id, cost, TargetOptions.Atm, MovementType.Expenditure);
            dbContext.SaveChanges();
        }

        public static void Credit(this AtmWithdrawModel atmWithdraw, ApplicationDbContext dbContext, decimal cost)
        {
            atmWithdraw.CurrentAmount += cost;
            dbContext.Entry(atmWithdraw).State = EntityState.Modified;
            HistoricMovementHelper.SaveCreditMovement(dbContext, atmWithdraw.Id, cost, TargetOptions.Atm, MovementType.Income);
            dbContext.SaveChanges();
        }
    }
}
