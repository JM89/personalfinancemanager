using PFM.DataAccessLayer.Enumerations;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events.Interfaces;
using System.Collections.Generic;

namespace PFM.Services.MovementStrategy
{
    public class ContextMovementStrategy
    {
        private Dictionary<PaymentMethod, MovementStrategy> _strategies;
        public ContextMovementStrategy(IBankAccountCache bankAccountCache, IIncomeRepository incomeRepository,
            IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
        {
            _strategies = new Dictionary<PaymentMethod, MovementStrategy>() {
                { PaymentMethod.Cash, new CashMovementStrategy(bankAccountCache, incomeRepository, atmWithdrawRepository, eventPublisher) },
                { PaymentMethod.InternalTransfer, new InternalTransferMovementStrategy(bankAccountCache, incomeRepository, atmWithdrawRepository, eventPublisher) },
                { PaymentMethod.CB, new CommonMovementStrategy(bankAccountCache, incomeRepository, atmWithdrawRepository, eventPublisher) },
                { PaymentMethod.DirectDebit, new CommonMovementStrategy(bankAccountCache, incomeRepository, atmWithdrawRepository, eventPublisher) },
                { PaymentMethod.Transfer, new CommonMovementStrategy(bankAccountCache, incomeRepository, atmWithdrawRepository, eventPublisher) }
            };
        }

        public MovementStrategy GetMovementStrategy(PaymentMethod paymentMethod)
        {
            return _strategies.GetValueOrDefault(paymentMethod);
        }
    }
}
