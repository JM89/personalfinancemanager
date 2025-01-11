using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.Interfaces;
using System.Threading.Tasks;
using PFM.Services.Caches;

namespace PFM.Services.MovementStrategy
{
    public abstract class MovementStrategy
    {
        protected readonly IBankAccountCache BankAccountCache;
        protected readonly IIncomeRepository IncomeRepository;
        protected readonly IAtmWithdrawRepository AtmWithdrawRepository;
        protected readonly IEventPublisher EventPublisher;
        protected readonly IExpenseTypeCache ExpenseTypeCache;

        protected MovementStrategy(IBankAccountCache bankAccountCache, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher, IExpenseTypeCache expenseTypeCache)
        { 
            this.BankAccountCache = bankAccountCache;
            this.IncomeRepository = incomeRepository;
            this.AtmWithdrawRepository = atmWithdrawRepository;
            this.EventPublisher = eventPublisher;
            this.ExpenseTypeCache = expenseTypeCache;
        }

        public abstract Task<bool> Debit(Movement movement);

        public abstract Task<bool> Credit(Movement movement);
    }
}
