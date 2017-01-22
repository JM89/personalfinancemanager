using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Helpers;
using System;

namespace PersonalFinanceManager.Services.MovementStrategy
{
    public class InternalTransferMovementStrategy : MovementStrategy
    {
        public InternalTransferMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository)
            : base(movement, bankAccountRepository, historicMovementRepository, incomeRepository, atmWithdrawRepository)
        { }

        public override void Debit()
        {
            if (CurrentMovement?.SourceAccountId != null && CurrentMovement.TargetAccountId.HasValue)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value);
                var internalAccount = BankAccountRepository.GetById(CurrentMovement.TargetAccountId.Value);
                Debit(account, internalAccount, CurrentMovement);
            }
            else
            {
                throw new Exception("Current movement / Source account / Target Account can't be null.");
            }
        }

        private void Debit(AccountModel account, AccountModel internalAccount, Movement movement)
        {
            MovementHelpers.Debit(HistoricMovementRepository, movement.Amount, account.Id, ObjectType.Account, account.CurrentBalance, internalAccount.Id, ObjectType.Account, internalAccount.CurrentBalance);

            account.CurrentBalance -= movement.Amount;
            BankAccountRepository.Update(account);

            internalAccount.CurrentBalance += movement.Amount;
            BankAccountRepository.Update(internalAccount);

            if (!movement.TargetAccountId.HasValue)
                throw new Exception("Target Income ID should not be null.");

            var incomeModel = new IncomeModel
            {
                Description = "Transfer: " + movement.Description,
                Cost = movement.Amount,
                AccountId = movement.TargetAccountId.Value,
                DateIncome = movement.Date
            };
            var income = IncomeRepository.Create(incomeModel);

            movement.TargetIncomeId = income.Id;
        }

        public override void Credit()
        {
            if (CurrentMovement?.SourceAccountId != null && CurrentMovement.TargetAccountId.HasValue)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value);
                var internalAccount = BankAccountRepository.GetById(CurrentMovement.TargetAccountId.Value);
                Credit(account, internalAccount, CurrentMovement);
            }
            else
            {
                throw new Exception("Current movement / Source account / Target Account can't be null.");
            }
        }

        private void Credit(AccountModel account, AccountModel internalAccount, Movement movement)
        {
            MovementHelpers.Credit(HistoricMovementRepository, movement.Amount, account.Id, ObjectType.Account, account.CurrentBalance, internalAccount.Id, ObjectType.Account, internalAccount.CurrentBalance);

            account.CurrentBalance += movement.Amount;
            BankAccountRepository.Update(account);

            internalAccount.CurrentBalance -= movement.Amount;
            BankAccountRepository.Update(internalAccount);

            if (!movement.TargetIncomeId.HasValue)
                throw new Exception("Target Income ID should not be null.");

            var income = IncomeRepository.GetById(movement.TargetIncomeId.Value);
            IncomeRepository.Delete(income);
        }

        public override void UpdateDebit(Movement newMovement)
        {
            if (CurrentMovement.SourceAccountId.HasValue && CurrentMovement.TargetAccountId.HasValue)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value);
                var internalAccount = BankAccountRepository.GetById(CurrentMovement.TargetAccountId.Value);

                if (CurrentMovement.PaymentMethod != newMovement.PaymentMethod)
                {
                    Credit(account, internalAccount, CurrentMovement);

                    var strategy = ContextMovementStrategy.GetMovementStrategy(newMovement, BankAccountRepository, HistoricMovementRepository, IncomeRepository, AtmWithdrawRepository);
                    strategy.Debit();
                }
                else
                {
                    if (!newMovement.TargetAccountId.HasValue)
                        throw new Exception("New Target account can't be null.");

                    if (CurrentMovement.TargetAccountId.Value != newMovement.TargetAccountId.Value)
                    {
                        var newInternalAccount = BankAccountRepository.GetById(newMovement.TargetAccountId.Value);
                        Credit(account, internalAccount, CurrentMovement);
                        Debit(account, newInternalAccount, newMovement);
                    }
                    else if (CurrentMovement.Amount != newMovement.Amount)
                    {
                        Credit(account, internalAccount, CurrentMovement);
                        Debit(account, internalAccount, newMovement);
                    }
                }
            }
            else
            {
                throw new Exception("Current Source account & Target Account can't be null.");
            }
        }
    }
}
