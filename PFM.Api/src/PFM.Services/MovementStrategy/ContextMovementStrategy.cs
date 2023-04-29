using PFM.DataAccessLayer.Enumerations;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.Interfaces;
using System;

namespace PFM.Services.MovementStrategy
{
    public static class ContextMovementStrategy
    {
        public static MovementStrategy GetMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IIncomeRepository incomeRepository,
            IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
        {
            MovementStrategy strategy = null;

            switch (movement.PaymentMethod)
            {
                case PaymentMethod.Cash:
                    strategy = new CashMovementStrategy(movement, bankAccountRepository, incomeRepository, atmWithdrawRepository, eventPublisher);
                    break;
                case PaymentMethod.InternalTransfer:
                    strategy = new InternalTransferMovementStrategy(movement, bankAccountRepository, incomeRepository, atmWithdrawRepository, eventPublisher);
                    break;
                case PaymentMethod.CB:
                case PaymentMethod.DirectDebit:
                case PaymentMethod.Transfer:
                    strategy = new CommonMovementStrategy(movement, bankAccountRepository, incomeRepository, atmWithdrawRepository, eventPublisher);
                    break;
                default:
                    throw new ArgumentException("Unknown PaymentMethod");
            }

            return strategy;
        }
    }
}
