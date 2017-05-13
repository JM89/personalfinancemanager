using PersonalFinanceManager.ServicesForTests;
using PersonalFinanceManager.ServicesForTests.Interfaces;

namespace PersonalFinanceManager.IntegrationTests.Infrastructure
{
    public static class DatabaseChecker
    {
        public static IBankAccountService BankAccountService;
        public static IAtmWithdrawService AtmWithdrawService;
        public static IHistoricMovementService HistoricMovementService;
        public static IExpenditureService ExpenditureService;
        public static IIncomeService IncomeService;
        public static ISavingService SavingService;

        public static void Initialize()
        {
            BankAccountService = new BankAccountService();
            AtmWithdrawService = new AtmWithdrawService();
            HistoricMovementService = new HistoricMovementService();
            ExpenditureService = new ExpenditureService();
            IncomeService = new IncomeService();
            SavingService = new SavingService();
        }
    }
}
