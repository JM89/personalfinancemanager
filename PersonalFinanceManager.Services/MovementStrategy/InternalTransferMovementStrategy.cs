using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Helpers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.MovementStrategy
{
    public class InternalTransferMovementStrategy : MovementStrategy
    {
        public InternalTransferMovementStrategy(Movement movement, IBankAccountRepository _bankAccountRepository, IHistoricMovementRepository _historicMovementRepository, IIncomeRepository _incomeRepository)
            : base(movement, _bankAccountRepository, _historicMovementRepository, _incomeRepository)
        { }

        public override void Debit()
        {
            var account = _bankAccountRepository.GetById(_currentMovement.SourceAccountId.Value);
            var internalAccount = _bankAccountRepository.GetById(_currentMovement.TargetAccountId.Value);
            Debit(account, internalAccount, _currentMovement);
        }

        public void Debit(AccountModel account, AccountModel internalAccount, Movement movement)
        {
            MovementHelpers.Debit(_historicMovementRepository, movement.Amount, account.Id, ObjectType.Account, account.CurrentBalance, internalAccount.Id, ObjectType.Account, internalAccount.CurrentBalance);

            account.CurrentBalance -= movement.Amount;
            _bankAccountRepository.Update(account);

            internalAccount.CurrentBalance += movement.Amount;
            _bankAccountRepository.Update(internalAccount);

            var incomeModel = new IncomeModel
            {
                Description = "Transfer: " + movement.Description,
                Cost = movement.Amount,
                AccountId = movement.TargetAccountId.Value,
                DateIncome = movement.Date
            };
            var income = _incomeRepository.Create(incomeModel);

            movement.TargetIncomeId = income.Id;
        }

        public override void Credit()
        {
            var account = _bankAccountRepository.GetById(_currentMovement.SourceAccountId.Value);
            var internalAccount = _bankAccountRepository.GetById(_currentMovement.TargetAccountId.Value);
            Credit(account, internalAccount, _currentMovement);
        }

        public void Credit(AccountModel account, AccountModel internalAccount, Movement movement)
        {
            MovementHelpers.Credit(_historicMovementRepository, movement.Amount, account.Id, ObjectType.Account, account.CurrentBalance, internalAccount.Id, ObjectType.Account, internalAccount.CurrentBalance);

            account.CurrentBalance += movement.Amount;
            _bankAccountRepository.Update(account);

            internalAccount.CurrentBalance -= movement.Amount;
            _bankAccountRepository.Update(internalAccount);

            var income = _incomeRepository.GetById(movement.TargetIncomeId.Value);
            _incomeRepository.Delete(income);
        }

        public override void UpdateDebit(Movement newMovement)
        {
            var account = _bankAccountRepository.GetById(newMovement.SourceAccountId.Value);
            var internalAccount = _bankAccountRepository.GetById(newMovement.TargetAccountId.Value);

            if (_currentMovement.PaymentMethod != newMovement.PaymentMethod)
            {
                Credit(account, internalAccount, _currentMovement);

                var strategy = ContextMovementStrategy.GetMovementStrategy(newMovement, _bankAccountRepository, _historicMovementRepository, _incomeRepository);
                strategy.Debit();
            }
            else if (_currentMovement.TargetAccountId.Value != newMovement.TargetAccountId.Value)
            {
                var oldInternalAccount = _bankAccountRepository.GetById(_currentMovement.TargetAccountId.Value);
                Credit(account, oldInternalAccount, _currentMovement);
                Debit(account, internalAccount, newMovement);
            }
            else if (_currentMovement.Amount != newMovement.Amount)
            {
                Credit(account, internalAccount, _currentMovement);
                Debit(account, internalAccount, newMovement);
            }
        }
    }
}
