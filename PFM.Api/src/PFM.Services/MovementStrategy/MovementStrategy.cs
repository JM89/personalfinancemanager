using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.Interfaces;

namespace PFM.Services.MovementStrategy
{
    public abstract class MovementStrategy
    {
        protected readonly IBankAccountRepository BankAccountRepository;
        protected readonly IHistoricMovementRepository HistoricMovementRepository;
        protected readonly IIncomeRepository IncomeRepository;
        protected readonly IAtmWithdrawRepository AtmWithdrawRepository;
        protected readonly IEventPublisher EventPublisher;

        protected Movement CurrentMovement;

        protected MovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
        { 
            CurrentMovement = movement;

            this.BankAccountRepository = bankAccountRepository;
            this.HistoricMovementRepository = historicMovementRepository;
            this.IncomeRepository = incomeRepository;
            this.AtmWithdrawRepository = atmWithdrawRepository;
            this.EventPublisher = eventPublisher;
        }

        public abstract void Debit();

        public abstract void Credit();

        public abstract void UpdateDebit(Movement newMovement);
    }
}
