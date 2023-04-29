using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Services.MovementStrategy
{
    public class CashMovementStrategy : MovementStrategy
    {
        public CashMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
            : base(movement, bankAccountRepository, incomeRepository, atmWithdrawRepository, eventPublisher)
        { }

        public override async Task<bool> Debit()
        {
            if (CurrentMovement?.AtmWithdrawId != null)
            {
                var atmWithdraw = AtmWithdrawRepository.GetById(CurrentMovement.AtmWithdrawId.Value);
                return await Debit(atmWithdraw, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("ATM Withdraw can't be null.");
            }
        }

        private Task<bool> Debit(AtmWithdraw atmWithdraw, Movement movement)
        {
            atmWithdraw.CurrentAmount -= movement.Amount;
            AtmWithdrawRepository.Update(atmWithdraw);

            return Task.FromResult(true);
        }

        public override async Task<bool> Credit()
        {
            if (CurrentMovement?.AtmWithdrawId != null)
            {
                var atmWithdraw = AtmWithdrawRepository.GetById(CurrentMovement.AtmWithdrawId.Value);
                return await Credit(atmWithdraw, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("ATM Withdraw can't be null.");
            }
        }

        private Task<bool> Credit(AtmWithdraw atmWithdraw, Movement movement)
        {
            atmWithdraw.CurrentAmount += movement.Amount;
            AtmWithdrawRepository.Update(atmWithdraw);

            return Task.FromResult(true);
        }
        
        public override async Task<bool> UpdateDebit(Movement newMovement)
        {
            if (CurrentMovement.AtmWithdrawId.HasValue)
            {
                var atmWithdraw = AtmWithdrawRepository.GetById(CurrentMovement.AtmWithdrawId.Value);

                if (CurrentMovement.PaymentMethod != newMovement.PaymentMethod)
                {
                    await Credit(atmWithdraw, CurrentMovement);

                    var strategy = ContextMovementStrategy.GetMovementStrategy(newMovement, BankAccountRepository, IncomeRepository, AtmWithdrawRepository, EventPublisher);
                    await strategy.Debit();
                }
                else
                {
                    if (!newMovement.AtmWithdrawId.HasValue)
                        throw new ArgumentException("New Target account can't be null.");

                    if (CurrentMovement.AtmWithdrawId.Value != newMovement.AtmWithdrawId.Value)
                    {
                        var newAtmWithdraw = AtmWithdrawRepository.GetById(newMovement.AtmWithdrawId.Value);
                        await Credit(atmWithdraw, CurrentMovement);
                        await Debit(newAtmWithdraw, newMovement);
                    }
                    else if (CurrentMovement.Amount != newMovement.Amount)
                    {
                        await Credit(atmWithdraw, CurrentMovement);
                        await Debit(atmWithdraw, newMovement);
                    }
                }
            }
            else
            {
                throw new ArgumentException("Current Source account & Target ATM can't be null.");
            }
            return true;
        }
    }
}
