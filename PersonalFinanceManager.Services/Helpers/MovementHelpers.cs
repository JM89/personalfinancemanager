using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Helpers
{
    public static class MovementHelpers
    {
        public static void DebitAccount(IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository,
            AccountModel account, decimal cost, MovementType mouvementType)
        {
            account.CurrentBalance -= cost;
            bankAccountRepository.Update(account);
            historicMovementRepository.SaveDebitMovement(account.Id, cost, TargetOptions.Account, mouvementType);
        }

        public static void CreditAccount(IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, 
            AccountModel account, decimal cost, MovementType mouvementType)
        {
            account.CurrentBalance += cost;
            bankAccountRepository.Update(account);
            historicMovementRepository.SaveCreditMovement(account.Id, cost, TargetOptions.Account, mouvementType);
        }

        public static void DebitAtmWithdraw(IAtmWithdrawRepository atmWithdrawRepository, IHistoricMovementRepository historicMovementRepository,
            AtmWithdrawModel atmWithdraw, decimal cost)
        {
            atmWithdraw.CurrentAmount -= cost;
            atmWithdrawRepository.Update(atmWithdraw);
            historicMovementRepository.SaveDebitMovement(atmWithdraw.Id, cost, TargetOptions.Atm, MovementType.Expenditure);
        }

        public static void CreditAtmWithdraw(IAtmWithdrawRepository atmWithdrawRepository, IHistoricMovementRepository historicMovementRepository,
            AtmWithdrawModel atmWithdraw, decimal cost)
        {
            atmWithdraw.CurrentAmount += cost;
            atmWithdrawRepository.Update(atmWithdraw);
            historicMovementRepository.SaveCreditMovement(atmWithdraw.Id, cost, TargetOptions.Atm, MovementType.Income);
        }
    }
}
