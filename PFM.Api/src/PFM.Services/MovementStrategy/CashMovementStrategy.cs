using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Services.MovementStrategy
{
    public class CashMovementStrategy : MovementStrategy
    {
        public CashMovementStrategy(IBankAccountCache bankAccountCache, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher, IExpenseTypeCache expenseTypeCache)
            : base(bankAccountCache, incomeRepository, atmWithdrawRepository, eventPublisher, expenseTypeCache)
        { }

        public override async Task<bool> Debit(Movement movement)
        {
            if (movement?.AtmWithdrawId != null)
            {
                var atmWithdraw = AtmWithdrawRepository.GetById(movement.AtmWithdrawId.Value);
                return await Debit(atmWithdraw, movement);
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

        public override async Task<bool> Credit(Movement movement)
        {
            if (movement?.AtmWithdrawId != null)
            {
                var atmWithdraw = AtmWithdrawRepository.GetById(movement.AtmWithdrawId.Value);
                return await Credit(atmWithdraw, movement);
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
    }
}
