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
    public class InternalTransferMovementStrategy : MovementStrategy
    {
        private readonly string OperationType = "Internal Transfer";

        public InternalTransferMovementStrategy(IBankAccountCache bankAccountCache, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
            : base(bankAccountCache, incomeRepository, atmWithdrawRepository, eventPublisher)
        { }

        public override async Task<bool> Debit(Movement movement)
        {
            if (movement?.SourceAccountId != null && movement.TargetAccountId.HasValue)
            {
                var account = await BankAccountCache.GetById(movement.SourceAccountId.Value);
                var internalAccount = await BankAccountCache.GetById(movement.TargetAccountId.Value);
                return await Debit(account, internalAccount, movement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account / Target Account can't be null.");
            }
        }

        private async Task<bool> Debit(AccountDetails account, AccountDetails internalAccount, Movement movement)
        {
            var evtDebited = new BankAccountDebited()
            {
                Id = account.Id,
                BankId = account.BankId,
                CurrencyId = account.CurrencyId,
                PreviousBalance = account.CurrentBalance,
                CurrentBalance = account.CurrentBalance - movement.Amount,
                UserId = account.OwnerId,
                OperationDate = movement.Date,
                OperationType = OperationType,
                TargetBankAccount = $"BankAccount-{internalAccount.Id.ToString("00000000")}"
            };

            account.CurrentBalance -= movement.Amount;

            var evtCredited = new BankAccountCredited()
            {
                Id = internalAccount.Id,
                BankId = internalAccount.BankId,
                CurrencyId = internalAccount.CurrencyId,
                PreviousBalance = internalAccount.CurrentBalance,
                CurrentBalance = internalAccount.CurrentBalance + movement.Amount,
                UserId = internalAccount.OwnerId,
                OperationDate = movement.Date,
                OperationType = OperationType,
                TargetBankAccount = $"BankAccount-{account.Id.ToString("00000000")}"
            };

            internalAccount.CurrentBalance += movement.Amount;

            if (!movement.TargetAccountId.HasValue)
                throw new ArgumentException("Target Income ID should not be null.");

            var income = new Income
            {
                Description = "Transfer: " + movement.Description,
                Cost = movement.Amount,
                AccountId = movement.TargetAccountId.Value,
                DateIncome = movement.Date
            };
            var mappedIncome = IncomeRepository.Create(income);

            movement.TargetIncomeId = mappedIncome.Id;

            var published = await EventPublisher.PublishAsync(evtDebited, default);
            if (published)
                published = await EventPublisher.PublishAsync(evtCredited, default);

            return published;
        }

        public override async Task<bool> Credit(Movement movement)
        {
            if (movement?.SourceAccountId != null && movement.TargetAccountId.HasValue)
            {
                var account = await BankAccountCache.GetById(movement.SourceAccountId.Value);
                var internalAccount = await BankAccountCache.GetById(movement.TargetAccountId.Value);
                return await Credit(account, internalAccount, movement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account / Target Account can't be null.");
            }
        }

        private async Task<bool> Credit(AccountDetails account, AccountDetails internalAccount, Movement movement)
        {
            var evtCredited = new BankAccountCredited()
            {
                Id = account.Id,
                BankId = account.BankId,
                CurrencyId = account.CurrencyId,
                PreviousBalance = account.CurrentBalance,
                CurrentBalance = account.CurrentBalance + movement.Amount,
                UserId = account.OwnerId,
                OperationDate = movement.Date,
                OperationType = OperationType,
                TargetBankAccount = $"BankAccount-{internalAccount.Id.ToString("00000000")}"
            };

            account.CurrentBalance += movement.Amount;

            var evtDebited = new BankAccountDebited()
            {
                Id = internalAccount.Id,
                BankId = internalAccount.BankId,
                CurrencyId = internalAccount.CurrencyId,
                PreviousBalance = internalAccount.CurrentBalance,
                CurrentBalance = internalAccount.CurrentBalance - movement.Amount,
                UserId = internalAccount.OwnerId,
                OperationDate = movement.Date,
                OperationType = OperationType,
                TargetBankAccount = $"BankAccount-{account.Id.ToString("00000000")}"
            };

            internalAccount.CurrentBalance -= movement.Amount;

            if (!movement.TargetIncomeId.HasValue)
                throw new ArgumentException("Target Income ID should not be null.");

            var income = IncomeRepository.GetById(movement.TargetIncomeId.Value);
            IncomeRepository.Delete(income);

            var published = await EventPublisher.PublishAsync(evtDebited, default);
            if (published)
                published = await EventPublisher.PublishAsync(evtCredited, default);

            return published;
        }
    }
}
