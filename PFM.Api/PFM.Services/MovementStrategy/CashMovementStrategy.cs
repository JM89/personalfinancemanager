using System;
using PFM.Services.Helpers;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Enumerations;
using PFM.DataAccessLayer.Repositories.Interfaces;

namespace PFM.Services.MovementStrategy
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
                throw new ArgumentException("ATM Withdraw can't be null.");
            }
        }

        private void Debit(AtmWithdraw atmWithdraw, Movement movement)
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
                throw new ArgumentException("ATM Withdraw can't be null.");
            }
        }

        private void Credit(AtmWithdraw atmWithdraw, Movement movement)
        {
            MovementHelpers.Credit(HistoricMovementRepository, movement.Amount, atmWithdraw.Id, ObjectType.AtmWithdraw, atmWithdraw.CurrentAmount);

            atmWithdraw.CurrentAmount += movement.Amount;
            AtmWithdrawRepository.Update(atmWithdraw);
        }
        
        public override void UpdateDebit(Movement newMovement)
        {
            if (CurrentMovement.AtmWithdrawId.HasValue)
            {
                var atmWithdraw = AtmWithdrawRepository.GetById(CurrentMovement.AtmWithdrawId.Value);

                if (CurrentMovement.PaymentMethod != newMovement.PaymentMethod)
                {
                    Credit(atmWithdraw, CurrentMovement);

                    var strategy = ContextMovementStrategy.GetMovementStrategy(newMovement, BankAccountRepository, HistoricMovementRepository, IncomeRepository, AtmWithdrawRepository);
                    strategy.Debit();
                }
                else
                {
                    if (!newMovement.AtmWithdrawId.HasValue)
                        throw new ArgumentException("New Target account can't be null.");

                    if (CurrentMovement.AtmWithdrawId.Value != newMovement.AtmWithdrawId.Value)
                    {
                        var newAtmWithdraw = AtmWithdrawRepository.GetById(newMovement.AtmWithdrawId.Value);
                        Credit(atmWithdraw, CurrentMovement);
                        Debit(newAtmWithdraw, newMovement);
                    }
                    else if (CurrentMovement.Amount != newMovement.Amount)
                    {
                        Credit(atmWithdraw, CurrentMovement);
                        Debit(atmWithdraw, newMovement);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Current Source account & Target ATM can't be null.");
            }
        }
    }
}
