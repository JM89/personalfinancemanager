using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.ExpenditureStrategy
{
    public static class ContextExpenditureStrategy
    {
        public static ExpenditureStrategy GetExpenditureStrategy(IBankAccountRepository bankAccountRepository, IAtmWithdrawRepository atmWithdrawRepository, IIncomeRepository incomeRepository,
            IHistoricMovementRepository historicMovementRepository, ExpenditureModel expenditureModel)
        {
            ExpenditureStrategy strategy;

            switch ((PaymentMethod)expenditureModel.PaymentMethodId)
            {
                case PaymentMethod.Cash:
                    strategy = new AtmWithdrawExpenditureStrategy(bankAccountRepository, atmWithdrawRepository, incomeRepository, historicMovementRepository, expenditureModel);
                    break;
                case PaymentMethod.InternalTransfer:
                    strategy = new InternalTransferExpenditureStrategy(bankAccountRepository, atmWithdrawRepository, incomeRepository, historicMovementRepository, expenditureModel);
                    break;
                case PaymentMethod.CB:
                case PaymentMethod.DirectDebit:
                case PaymentMethod.Transfer:
                    strategy = new CommonExpenditureStrategy(bankAccountRepository, atmWithdrawRepository, incomeRepository, historicMovementRepository, expenditureModel);
                    break;
                default:
                    throw new ArgumentException("Unknown PaymentMethod");
            }

            return strategy;
        }
    }
}
