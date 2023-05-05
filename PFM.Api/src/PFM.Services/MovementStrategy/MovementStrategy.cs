using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events.Interfaces;
using System.Threading.Tasks;

namespace PFM.Services.MovementStrategy
{
    public abstract class MovementStrategy
    {
        protected readonly IBankAccountCache BankAccountCache;
        protected readonly IIncomeRepository IncomeRepository;
        protected readonly IAtmWithdrawRepository AtmWithdrawRepository;
        protected readonly IEventPublisher EventPublisher;

        protected Movement CurrentMovement;

        protected MovementStrategy(Movement movement, IBankAccountCache bankAccountCache, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
        { 
            CurrentMovement = movement;

            this.BankAccountCache = bankAccountCache;
            this.IncomeRepository = incomeRepository;
            this.AtmWithdrawRepository = atmWithdrawRepository;
            this.EventPublisher = eventPublisher;
        }

        public abstract Task<bool> Debit();

        public abstract Task<bool> Credit();
    }
}
