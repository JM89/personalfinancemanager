using PersonalFinanceManager.DataAccess.Repositories.Interfaces;

namespace PersonalFinanceManager.Services.MovementStrategy
{
    public abstract class MovementStrategy
    {
        protected readonly IBankAccountRepository BankAccountRepository;
        protected readonly IHistoricMovementRepository HistoricMovementRepository;
        protected readonly IIncomeRepository IncomeRepository;
        protected readonly IAtmWithdrawRepository AtmWithdrawRepository;

        protected Movement CurrentMovement;

        protected MovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository)
        { 
            CurrentMovement = movement;

            this.BankAccountRepository = bankAccountRepository;
            this.HistoricMovementRepository = historicMovementRepository;
            this.IncomeRepository = incomeRepository;
            this.AtmWithdrawRepository = atmWithdrawRepository;
        }

        public abstract void Debit();

        public abstract void Credit();

        public abstract void UpdateDebit(Movement newMovement);
    }
}
