using PFM.DataAccessLayer.Enumerations;
using PFM.DataAccessLayer.Repositories.Interfaces;
using System;

namespace PFM.Services.MovementStrategy
{
    public static class ContextMovementStrategy
    {
        public static MovementStrategy GetMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository)
        {
            MovementStrategy strategy = null;

            switch (movement.PaymentMethod)
            {
                case PaymentMethod.Cash:
                    strategy = new CashMovementStrategy(movement, bankAccountRepository, historicMovementRepository, incomeRepository, atmWithdrawRepository);
                    break;
                case PaymentMethod.InternalTransfer:
                    strategy = new InternalTransferMovementStrategy(movement, bankAccountRepository, historicMovementRepository, incomeRepository, atmWithdrawRepository);
                    break;
                case PaymentMethod.CB:
                case PaymentMethod.DirectDebit:
                case PaymentMethod.Transfer:
                    strategy = new CommonMovementStrategy(movement, bankAccountRepository, historicMovementRepository, incomeRepository, atmWithdrawRepository);
                    break;
                default:
                    throw new ArgumentException("Unknown PaymentMethod");
            }

            return strategy;
        }
    }
}
