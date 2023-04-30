using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Enumerations;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.EventTypes;
using PFM.Services.Events.Interfaces;
using System;
using System.Threading.Tasks;

namespace PFM.Services.MovementStrategy
{
    public class CommonMovementStrategy : MovementStrategy
    {
        public CommonMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
            : base(movement, bankAccountRepository, incomeRepository, atmWithdrawRepository, eventPublisher)
        { }

        public override async Task<bool> Debit()
        {
            if (CurrentMovement?.SourceAccountId != null)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value, a => a.Currency, a => a.Bank);
                return await Debit(account, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account can't be null.");
            }
        }

        private async Task<bool> Debit(Account account, Movement movement)
        {
            var evt = new BankAccountDebited()
            {
                BankCode = account.Bank.Id.ToString(),
                CurrencyCode = account.Currency.Id.ToString(),
                PreviousBalance = account.CurrentBalance,
                CurrentBalance = account.CurrentBalance - movement.Amount,
                UserId = account.User_Id,
                OperationDate = movement.Date,
                OperationType = $"Expense via {movement.PaymentMethod}"
            };

            account.CurrentBalance -= movement.Amount;
            BankAccountRepository.Update(account);

            return await EventPublisher.PublishAsync(evt, default);
        }

        public override async Task<bool> Credit()
        {
            if (CurrentMovement?.SourceAccountId != null)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value, a => a.Currency, a => a.Bank);
                return await Credit(account, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account can't be null.");
            }
        }

        private async Task<bool> Credit(Account account, Movement movement)
        {
            var evt = new BankAccountCredited()
            {
                BankCode = account.Bank.Id.ToString(),
                CurrencyCode = account.Currency.Id.ToString(),
                PreviousBalance = account.CurrentBalance,
                CurrentBalance = account.CurrentBalance + movement.Amount,
                UserId = account.User_Id,
                OperationDate = movement.Date,
                OperationType = $"Expense via {movement.PaymentMethod}"
            };

            account.CurrentBalance += movement.Amount;
            BankAccountRepository.Update(account);

            return await EventPublisher.PublishAsync(evt, default);
        }
    }
}
