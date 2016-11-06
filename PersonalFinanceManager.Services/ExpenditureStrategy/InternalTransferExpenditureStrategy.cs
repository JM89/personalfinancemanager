using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using PersonalFinanceManager.Services.Extensions;
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
        public InternalTransferExpenditureStrategy(ApplicationDbContext dbContext, ExpenditureModel expenditureModel)
            : base(dbContext, expenditureModel)
        { }

        public override void Debit()
        {
            var accountModel = _dbContext.AccountModels.SingleOrDefault(x => x.Id == _expenditureModel.AccountId);
            accountModel.Debit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);

            if (!_expenditureModel.TargetInternalAccountId.HasValue)
                throw new ArgumentException("For an internal transfer, we should have a TargetInternalAccountId");

            CreateIncomeForTransfer(_expenditureModel);

            var targetAccountModel = _dbContext.AccountModels.SingleOrDefault(x => x.Id == _expenditureModel.TargetInternalAccountId);
            targetAccountModel.Credit(_dbContext, _expenditureModel.Cost, MovementType.Income);
        }

        public override void Credit()
        {
            var accountModel = _dbContext.AccountModels.SingleOrDefault(x => x.Id == _expenditureModel.AccountId);
            accountModel.Credit(_dbContext, _expenditureModel.Cost, MovementType.Income);

            if (!_expenditureModel.TargetInternalAccountId.HasValue)
                throw new ArgumentException("For an internal transfer, we should have a TargetInternalAccountId");

            RemoveIncome();

            var targetAccountModel = _dbContext.AccountModels.SingleOrDefault(x => x.Id == _expenditureModel.TargetInternalAccountId);
            targetAccountModel.Debit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);
        }

        public override void UpdateDebit(ExpenditureModel newExpenditure)
        {
            if (_expenditureModel.PaymentMethodId != newExpenditure.PaymentMethodId)
            {
                var account = _dbContext.AccountModels.Single(x => x.Id == _expenditureModel.AccountId);
                account.Credit(_dbContext, _expenditureModel.Cost, MovementType.Income);

                var internalAccount = _dbContext.AccountModels.Single(x => x.Id == _expenditureModel.TargetInternalAccountId);
                internalAccount.Debit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);

                RemoveIncome();

                var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_dbContext, newExpenditure);
                strategy.Debit();
            }
            else
            {
                if (_expenditureModel.Cost != newExpenditure.Cost)
                {
                    var account = _dbContext.AccountModels.Single(x => x.Id == newExpenditure.AccountId);
                    var internalAccount = _dbContext.AccountModels.Single(x => x.Id == newExpenditure.TargetInternalAccountId);
                    if (_expenditureModel.TargetInternalAccountId != newExpenditure.TargetInternalAccountId)
                    {
                        account.Credit(_dbContext, _expenditureModel.Cost, MovementType.Income);
                        account.Debit(_dbContext, newExpenditure.Cost, MovementType.Expenditure);

                        var oldInternalAccount = _dbContext.AccountModels.Single(x => x.Id == _expenditureModel.TargetInternalAccountId);
                        oldInternalAccount.Debit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);
                        internalAccount.Credit(_dbContext, newExpenditure.Cost, MovementType.Income);

                        RemoveIncome();

                        CreateIncomeForTransfer(newExpenditure);
                    }
                    else
                    {
                        account.Credit(_dbContext, _expenditureModel.Cost, MovementType.Income);
                        account.Debit(_dbContext, newExpenditure.Cost, MovementType.Expenditure);
                        
                        internalAccount.Debit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);
                        internalAccount.Credit(_dbContext, newExpenditure.Cost, MovementType.Income);

                        var income = GetIncome(_expenditureModel);
                        income.Cost = newExpenditure.Cost;
                        _dbContext.SaveChanges();
                    }
                }
            }
        }

        private IncomeModel GetIncome(ExpenditureModel transfer)
        {
            var income = _dbContext.IncomeModels.Single(x =>
                            x.Cost == transfer.Cost &&
                            x.DateIncome == transfer.DateExpenditure &&
                            x.Description == "Transfer: " + transfer.Description);

            return income;
        }

        private void RemoveIncome()
        {
            var income = GetIncome(_expenditureModel);
            _dbContext.IncomeModels.Remove(income);
            _dbContext.SaveChanges();
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
            _dbContext.IncomeModels.Add(incomeModel);
            _dbContext.SaveChanges();
        }
    }
}
