using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.MovementStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.MovementStrategy
{
    public static class ContextMovementStrategy
    {
        public static MovementStrategy GetMovementStrategy(Movement movement, IBankAccountRepository _bankAccountRepository, IHistoricMovementRepository _historicMovementRepository, IIncomeRepository _incomeRepository)
        {
            MovementStrategy strategy = null;

            switch (movement.PaymentMethod)
            {
                case PaymentMethod.Cash:
                    strategy = new AtmWithdrawMovementStrategy(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository);
                    break;
                case PaymentMethod.InternalTransfer:
                    strategy = new InternalTransferMovementStrategy(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository);
                    break;
                case PaymentMethod.CB:
                case PaymentMethod.DirectDebit:
                case PaymentMethod.Transfer:
                    strategy = new CommonMovementStrategy(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository);
                    break;
                default:
                    throw new ArgumentException("Unknown PaymentMethod");
            }

            return strategy;
        }
    }
}
