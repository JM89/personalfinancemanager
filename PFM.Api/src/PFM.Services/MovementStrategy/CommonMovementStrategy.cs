using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.Enumerations;
using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.Services.Events.EventTypes;
using PFM.Services.Events.Interfaces;
using PFM.Services.Helpers;
using System;
using System.Threading.Tasks;

namespace PFM.Services.MovementStrategy
{
    public class CommonMovementStrategy : MovementStrategy
    {
        public CommonMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository, IEventPublisher eventPublisher)
            : base(movement, bankAccountRepository, historicMovementRepository, incomeRepository, atmWithdrawRepository, eventPublisher)
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
            MovementHelpers.Debit(HistoricMovementRepository, movement.Amount, account.Id, ObjectType.Account, account.CurrentBalance);

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
            MovementHelpers.Credit(HistoricMovementRepository, movement.Amount, account.Id, ObjectType.Account, account.CurrentBalance);

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

        public override async Task<bool> UpdateDebit(Movement newMovement)
        {
            if (newMovement.SourceAccountId.HasValue)
            {
                var account = BankAccountRepository.GetById(newMovement.SourceAccountId.Value, a => a.Currency, a => a.Bank);
                if (CurrentMovement.PaymentMethod != newMovement.PaymentMethod)
                {
                    await Credit(account, CurrentMovement);

                    var strategy = ContextMovementStrategy.GetMovementStrategy(newMovement, BankAccountRepository,
                        HistoricMovementRepository, IncomeRepository, AtmWithdrawRepository, EventPublisher);

                    await strategy.Debit();
                }
                else if (CurrentMovement.Amount != newMovement.Amount)
                {
                    await Credit(account, CurrentMovement);
                    await Debit(account, newMovement);
                }

                return true;
            }
            else
            {
                throw new ArgumentException("Current Source account can't be null.");
            }
        }
    }
}
