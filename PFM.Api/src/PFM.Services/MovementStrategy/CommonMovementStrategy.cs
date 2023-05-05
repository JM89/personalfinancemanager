using PFM.Bank.Api.Contracts.Account;
using PFM.Bank.Event.Contracts;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Caches.Interfaces;
using PFM.Services.Events.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Services.MovementStrategy
{
    public class CommonMovementStrategy : MovementStrategy
    {
        public CommonMovementStrategy(Movement movement, IBankAccountCache bankAccountCache, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
            : base(movement, bankAccountCache, incomeRepository, atmWithdrawRepository, eventPublisher)
        { }

        public override async Task<bool> Debit()
        {
            if (CurrentMovement?.SourceAccountId != null)
            {
                var account = await BankAccountCache.GetById(CurrentMovement.SourceAccountId.Value);
                return await Debit(account, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account can't be null.");
            }
        }

        private async Task<bool> Debit(AccountDetails account, Movement movement)
        {
            var evt = new BankAccountDebited()
            {
                Id = account.Id,
                BankId = account.BankId,
                CurrencyId = account.CurrencyId,
                PreviousBalance = account.CurrentBalance,
                CurrentBalance = account.CurrentBalance - movement.Amount,
                UserId = account.OwnerId,
                OperationDate = movement.Date,
                OperationType = $"Expense via {movement.PaymentMethod}"
            };

            account.CurrentBalance -= movement.Amount;

            return await EventPublisher.PublishAsync(evt, default);
        }

        public override async Task<bool> Credit()
        {
            if (CurrentMovement?.SourceAccountId != null)
            {
                var account = await BankAccountCache.GetById(CurrentMovement.SourceAccountId.Value);
                return await Credit(account, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account can't be null.");
            }
        }

        private async Task<bool> Credit(AccountDetails account, Movement movement)
        {
            var evt = new BankAccountCredited()
            {
                Id = account.Id,
                BankId = account.BankId,
                CurrencyId = account.CurrencyId,
                PreviousBalance = account.CurrentBalance,
                CurrentBalance = account.CurrentBalance + movement.Amount,
                UserId = account.OwnerId,
                OperationDate = movement.Date,
                OperationType = $"Expense via {movement.PaymentMethod}"
            };

            account.CurrentBalance += movement.Amount;

            return await EventPublisher.PublishAsync(evt, default);
        }
    }
}
