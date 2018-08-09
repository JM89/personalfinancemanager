using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.DataAccess.Repositories;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure
{
    public static class DatabaseChecker
    {
        public static IBankAccountRepository BankAccountRepository;
        public static IAtmWithdrawRepository AtmWithdrawRepository;
        public static IHistoricMovementRepository HistoricMovementRepository;
        public static IExpenditureRepository ExpenditureRepository;
        public static IIncomeRepository IncomeRepository;
        public static ISavingRepository SavingRepository;

        public static void Initialize()
        {
            var ctx = new ApplicationDbContext();
            BankAccountRepository = new BankAccountRepository(ctx);
            AtmWithdrawRepository = new AtmWithdrawRepository(ctx);
            HistoricMovementRepository = new HistoricMovementRepository(ctx);
            ExpenditureRepository = new ExpenditureRepository(ctx);
            IncomeRepository = new IncomeRepository(ctx);
            SavingRepository = new SavingRepository(ctx);
        }
    }
}
