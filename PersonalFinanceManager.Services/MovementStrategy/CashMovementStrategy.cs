using System;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Helpers;

namespace PersonalFinanceManager.Services.MovementStrategy
{
    public class CashMovementStrategy : MovementStrategy
    {
        public CashMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository)
            : base(movement, bankAccountRepository, historicMovementRepository, incomeRepository, atmWithdrawRepository)
        { }

        public override void Debit()
        {
            if (CurrentMovement?.AtmWithdrawId != null)
            {
                var atmWithdraw = AtmWithdrawRepository.GetById(CurrentMovement.AtmWithdrawId.Value);
                Debit(atmWithdraw, CurrentMovement);
            }
            else
            {
                throw new Exception("ATM Withdraw can't be null.");
            }
        }

        public void Debit(AtmWithdrawModel atmWithdraw, Movement movement)
        {
            MovementHelpers.Debit(HistoricMovementRepository, movement.Amount, atmWithdraw.Id, ObjectType.AtmWithdraw, atmWithdraw.CurrentAmount);

            atmWithdraw.CurrentAmount -= movement.Amount;
            AtmWithdrawRepository.Update(atmWithdraw);
        }

        public override void Credit()
        {
            if (CurrentMovement?.AtmWithdrawId != null)
            {
                var atmWithdraw = AtmWithdrawRepository.GetById(CurrentMovement.AtmWithdrawId.Value);
                Credit(atmWithdraw, CurrentMovement);
            }
            else
            {
                throw new Exception("ATM Withdraw can't be null.");
            }
        }

        public void Credit(AtmWithdrawModel atmWithdraw, Movement movement)
        {
            MovementHelpers.Credit(HistoricMovementRepository, movement.Amount, atmWithdraw.Id, ObjectType.AtmWithdraw, atmWithdraw.CurrentAmount);

            atmWithdraw.CurrentAmount += movement.Amount;
            AtmWithdrawRepository.Update(atmWithdraw);
        }
        
        public override void UpdateDebit(Movement newMovement)
        {
            
        }
    }
}
