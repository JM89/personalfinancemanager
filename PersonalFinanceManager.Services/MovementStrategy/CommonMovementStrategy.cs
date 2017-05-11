﻿using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Helpers;
using System;

namespace PersonalFinanceManager.Services.MovementStrategy
{
    public class CommonMovementStrategy : MovementStrategy
    {
        public CommonMovementStrategy(Movement movement, IBankAccountRepository bankAccountRepository, IHistoricMovementRepository historicMovementRepository, IIncomeRepository incomeRepository, IAtmWithdrawRepository atmWithdrawRepository)
            : base(movement, bankAccountRepository, historicMovementRepository, incomeRepository, atmWithdrawRepository)
        { }

        public override void Debit()
        {
            if (CurrentMovement?.SourceAccountId != null)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value);
                Debit(account, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account can't be null.");
            }
        }

        private void Debit(AccountModel account, Movement movement)
        {
            MovementHelpers.Debit(HistoricMovementRepository, movement.Amount, account.Id, ObjectType.Account, account.CurrentBalance);

            account.CurrentBalance -= movement.Amount;
            BankAccountRepository.Update(account);
        }

        public override void Credit()
        {
            if (CurrentMovement?.SourceAccountId != null)
            {
                var account = BankAccountRepository.GetById(CurrentMovement.SourceAccountId.Value);
                Credit(account, CurrentMovement);
            }
            else
            {
                throw new ArgumentException("Current movement / Source account can't be null.");
            }
        }

        private void Credit(AccountModel account, Movement movement)
        {
            MovementHelpers.Credit(HistoricMovementRepository, movement.Amount, account.Id, ObjectType.Account, account.CurrentBalance);

            account.CurrentBalance += movement.Amount;
            BankAccountRepository.Update(account);
        }

        public override void UpdateDebit(Movement newMovement)
        {
            if (newMovement.SourceAccountId.HasValue)
            {
                var account = BankAccountRepository.GetById(newMovement.SourceAccountId.Value);
                if (CurrentMovement.PaymentMethod != newMovement.PaymentMethod)
                {
                    Credit(account, CurrentMovement);

                    var strategy = ContextMovementStrategy.GetMovementStrategy(newMovement, BankAccountRepository,
                        HistoricMovementRepository, IncomeRepository, AtmWithdrawRepository);

                    strategy.Debit();
                }
                else if (CurrentMovement.Amount != newMovement.Amount)
                {
                    Credit(account, CurrentMovement);
                    Debit(account, newMovement);
                }
            }
            else
            {
                throw new ArgumentException("Current Source account can't be null.");
            }
        }
    }
}
