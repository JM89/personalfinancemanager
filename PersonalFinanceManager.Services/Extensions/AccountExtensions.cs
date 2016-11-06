using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.ExpenditureStrategy;
using System.Data.Entity;

namespace PersonalFinanceManager.Services.Extensions
{
    public static class AccountExtensions
    {
        public static void Debit(this AccountModel account, ApplicationDbContext dbContext, decimal cost, MovementType mouvementType)
        {
            account.CurrentBalance -= cost;
            dbContext.Entry(account).State = EntityState.Modified;
            HistoricMovementHelper.SaveDebitMovement(dbContext, account.Id, cost, TargetOptions.Account, mouvementType);
            dbContext.SaveChanges();
        }

        public static void Credit(this AccountModel account, ApplicationDbContext dbContext, decimal cost, MovementType mouvementType)
        {
            account.CurrentBalance += cost;
            dbContext.Entry(account).State = EntityState.Modified;
            HistoricMovementHelper.SaveCreditMovement(dbContext, account.Id, cost, TargetOptions.Account, mouvementType);
            dbContext.SaveChanges();
        }
    }
}
