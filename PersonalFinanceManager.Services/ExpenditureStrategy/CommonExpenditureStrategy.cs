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
    public class CommonExpenditureStrategy : ExpenditureStrategy
    {
        public CommonExpenditureStrategy(ApplicationDbContext dbContext, ExpenditureModel expenditureModel)
            : base(dbContext, expenditureModel)
        { }

        public override void Debit()
        {
            var account = _dbContext.AccountModels.Single(x => x.Id == _expenditureModel.AccountId);
            account.Debit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);
        }

        public override void Credit()
        {
            var account = _dbContext.AccountModels.Single(x => x.Id == _expenditureModel.AccountId);
            account.Credit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);
        }

        public override void UpdateDebit(ExpenditureModel newExpenditure)
        {
            if (_expenditureModel.PaymentMethodId != newExpenditure.PaymentMethodId)
            {
                var account = _dbContext.AccountModels.Single(x => x.Id == _expenditureModel.AccountId);
                account.Credit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);

                var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_dbContext, newExpenditure);
                strategy.Debit();
            }
            else
            {
                if (_expenditureModel.Cost != newExpenditure.Cost)
                {
                    var account = _dbContext.AccountModels.Single(x => x.Id == _expenditureModel.AccountId);
                    account.Credit(_dbContext, _expenditureModel.Cost, MovementType.Expenditure);
                    account.Debit(_dbContext, newExpenditure.Cost, MovementType.Expenditure);
                }
            }
        }
    }
}
