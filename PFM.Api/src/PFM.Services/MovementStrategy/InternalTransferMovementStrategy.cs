using PFM.Bank.Event.Contracts;
using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Services.MovementStrategy
{
    public class InternalTransferMovementStrategy : MovementStrategy
    {
        private readonly string OperationType = "Internal Transfer";

        public InternalTransferMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
            : base(movement, bankAccountRepository, incomeRepository, atmWithdrawRepository, eventPublisher)
        { }

        public override async Task<bool> Debit()
        {
            if (CurrentMovement?.SourceAccountId != null && CurrentMovement.TargetAccountId.HasValue)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value, a => a.Currency, a => a.Bank);
                var internalAccount = BankAccountRepository.GetById(CurrentMovement.TargetAccountId.Value, a => a.Currency, a => a.Bank);
                return await Debit(account, internalAccount, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account / Target Account can't be null.");
            }
        }

        private async Task<bool> Debit(Account account, Account internalAccount, Movement movement)
        {
            var evtDebited = new BankAccountDebited()
            {
                BankCode = account.Bank.Id.ToString(),
                CurrencyCode = account.Currency.Id.ToString(),
                PreviousBalance = account.CurrentBalance,
                CurrentBalance = account.CurrentBalance - movement.Amount,
                UserId = account.User_Id,
                OperationDate = movement.Date,
                OperationType = OperationType,
                TargetBankAccount = $"BankAccount-{internalAccount.User_Id}-{internalAccount.Bank.Id}-{internalAccount.Currency.Id}"
            };

            account.CurrentBalance -= movement.Amount;
            BankAccountRepository.Update(account);

            var evtCredited = new BankAccountCredited()
            {
                BankCode = internalAccount.Bank.Id.ToString(),
                CurrencyCode = internalAccount.Currency.Id.ToString(),
                PreviousBalance = internalAccount.CurrentBalance,
                CurrentBalance = internalAccount.CurrentBalance + movement.Amount,
                UserId = internalAccount.User_Id,
                OperationDate = movement.Date,
                OperationType = OperationType,
                TargetBankAccount = $"BankAccount-{account.User_Id}-{account.Bank.Id}-{account.Currency.Id}"
            };

            internalAccount.CurrentBalance += movement.Amount;
            BankAccountRepository.Update(internalAccount);

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

        public override async Task<bool> Credit()
        {
            if (CurrentMovement?.SourceAccountId != null && CurrentMovement.TargetAccountId.HasValue)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value, a => a.Currency, a => a.Bank);
                var internalAccount = BankAccountRepository.GetById(CurrentMovement.TargetAccountId.Value, a => a.Currency, a => a.Bank);
                return await Credit(account, internalAccount, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account / Target Account can't be null.");
            }
        }

        private async Task<bool> Credit(Account account, Account internalAccount, Movement movement)
        {
            var evtCredited = new BankAccountCredited()
            {
                BankCode = account.Bank.Id.ToString(),
                CurrencyCode = account.Currency.Id.ToString(),
                PreviousBalance = account.CurrentBalance,
                CurrentBalance = account.CurrentBalance + movement.Amount,
                UserId = account.User_Id,
                OperationDate = movement.Date,
                OperationType = OperationType,
                TargetBankAccount = $"BankAccount-{internalAccount.User_Id}-{internalAccount.Bank.Id}-{internalAccount.Currency.Id}"
            };

            account.CurrentBalance += movement.Amount;
            BankAccountRepository.Update(account);

            var evtDebited = new BankAccountDebited()
            {
                BankCode = internalAccount.Bank.Id.ToString(),
                CurrencyCode = internalAccount.Currency.Id.ToString(),
                PreviousBalance = internalAccount.CurrentBalance,
                CurrentBalance = internalAccount.CurrentBalance - movement.Amount,
                UserId = internalAccount.User_Id,
                OperationDate = movement.Date,
                OperationType = OperationType,
                TargetBankAccount = $"BankAccount-{account.User_Id}-{account.Bank.Id}-{account.Currency.Id}"
            };

            internalAccount.CurrentBalance -= movement.Amount;
            BankAccountRepository.Update(internalAccount);

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
