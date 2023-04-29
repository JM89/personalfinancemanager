using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.Interfaces;
using System.Threading.Tasks;

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

        public abstract Task<bool> Debit();

        public abstract Task<bool> Credit();

        public abstract Task<bool> UpdateDebit(Movement newMovement);
    }
}
