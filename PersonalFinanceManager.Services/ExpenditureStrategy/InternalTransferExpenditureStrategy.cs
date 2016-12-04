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

namespace PersonalFinanceManager.Services.ExpenditureStrategy
{
    public class InternalTransferExpenditureStrategy : ExpenditureStrategy
    {
        public InternalTransferExpenditureStrategy(IBankAccountRepository bankAccountRepository, IAtmWithdrawRepository atmWithdrawRepository, IIncomeRepository incomeRepository,
            IHistoricMovementRepository historicMovementRepository, ExpenditureModel expenditureModel)
            : base(bankAccountRepository, atmWithdrawRepository, incomeRepository, historicMovementRepository, expenditureModel)
        { }

        public override void Debit()
        {
            var accountModel = _bankAccountRepository.GetById(_expenditureModel.AccountId);
            MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, accountModel, _expenditureModel.Cost, MovementType.Expenditure);

            if (!_expenditureModel.TargetInternalAccountId.HasValue)
                throw new ArgumentException("For an internal transfer, we should have a TargetInternalAccountId");

            CreateIncomeForTransfer(_expenditureModel);

            var targetAccountModel = _bankAccountRepository.GetById(_expenditureModel.TargetInternalAccountId.Value);
            MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, targetAccountModel, _expenditureModel.Cost, MovementType.Income);
        }

        public override void Credit()
        {
            var accountModel = _bankAccountRepository.GetById(_expenditureModel.AccountId);
            MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, accountModel, _expenditureModel.Cost, MovementType.Income);

            if (!_expenditureModel.TargetInternalAccountId.HasValue)
                throw new ArgumentException("For an internal transfer, we should have a TargetInternalAccountId");

            RemoveIncome();

            var targetAccountModel = _bankAccountRepository.GetById(_expenditureModel.TargetInternalAccountId.Value);
            MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, targetAccountModel, _expenditureModel.Cost, MovementType.Expenditure);
        }

        public override void UpdateDebit(ExpenditureModel newExpenditure)
        {
            if (_expenditureModel.PaymentMethodId != newExpenditure.PaymentMethodId)
            {
                var account = _bankAccountRepository.GetById(_expenditureModel.AccountId);
                MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, account, _expenditureModel.Cost, MovementType.Income);

                var internalAccount = _bankAccountRepository.GetById(_expenditureModel.TargetInternalAccountId.Value);
                MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, internalAccount, _expenditureModel.Cost, MovementType.Expenditure);

                RemoveIncome();

                var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_bankAccountRepository, _atmWithdrawRepository, _incomeRepository, _historicMovementRepository, newExpenditure);
                strategy.Debit();
            }
            else
            {
                if (_expenditureModel.Cost != newExpenditure.Cost)
                {
                    var account = _bankAccountRepository.GetById(newExpenditure.AccountId);
                    var internalAccount = _bankAccountRepository.GetById(newExpenditure.TargetInternalAccountId.Value);
                    if (_expenditureModel.TargetInternalAccountId != newExpenditure.TargetInternalAccountId)
                    {
                        MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, account, _expenditureModel.Cost, MovementType.Income);
                        MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, account, newExpenditure.Cost, MovementType.Expenditure);

                        var oldInternalAccount = _bankAccountRepository.GetById(_expenditureModel.TargetInternalAccountId.Value);
                        MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, oldInternalAccount, _expenditureModel.Cost, MovementType.Expenditure);
                        MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, internalAccount, newExpenditure.Cost, MovementType.Income);

                        RemoveIncome();

                        CreateIncomeForTransfer(newExpenditure);
                    }
                    else
                    {
                        MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, account, _expenditureModel.Cost, MovementType.Income);
                        MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, account, newExpenditure.Cost, MovementType.Expenditure);
                        
                        MovementHelpers.DebitAccount(_bankAccountRepository, _historicMovementRepository, internalAccount, _expenditureModel.Cost, MovementType.Expenditure);
                        MovementHelpers.CreditAccount(_bankAccountRepository, _historicMovementRepository, internalAccount, newExpenditure.Cost, MovementType.Income);

                        var income = GetIncome(_expenditureModel);
                        income.Cost = newExpenditure.Cost;
                    }
                }
            }
        }

        private IncomeModel GetIncome(ExpenditureModel transfer)
        {
            var income = _incomeRepository.GetList().Single(x =>
                            x.Cost == transfer.Cost &&
                            x.DateIncome == transfer.DateExpenditure &&
                            x.Description == "Transfer: " + transfer.Description);

            return income;
        }

        private void RemoveIncome()
        {
            var income = GetIncome(_expenditureModel);
            _incomeRepository.Delete(income);
        }

        private void CreateIncomeForTransfer(ExpenditureModel expenditureModel)
        {
            var incomeModel = new IncomeModel
            {
                Description = "Transfer: " + expenditureModel.Description,
                Cost = expenditureModel.Cost,
                AccountId = expenditureModel.TargetInternalAccountId.Value,
                DateIncome = expenditureModel.DateExpenditure
            };
            _incomeRepository.Create(incomeModel);
        }
    }
}
