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
    public class AtmWithdrawExpenditureStrategy : ExpenditureStrategy
    {
        public AtmWithdrawExpenditureStrategy(ApplicationDbContext dbContext, ExpenditureModel expenditureModel)
            : base(dbContext, expenditureModel)
        { }

        public override void Debit()
        {
            var atmWithdrawModel = _dbContext.AtmWithdrawModels.Single(x => x.Id == _expenditureModel.AtmWithdrawId);
            atmWithdrawModel.Debit(_dbContext, _expenditureModel.Cost);
        }

        public override void Credit()
        {
            var atmWithdrawModel = _dbContext.AtmWithdrawModels.Single(x => x.Id == _expenditureModel.AtmWithdrawId);
            atmWithdrawModel.Credit(_dbContext, _expenditureModel.Cost);
        }

        public override void UpdateDebit(ExpenditureModel newExpenditure)
        {
            if (_expenditureModel.PaymentMethodId != newExpenditure.PaymentMethodId)
            {
                var atm = _dbContext.AtmWithdrawModels.Single(x => x.Id == newExpenditure.AtmWithdrawId.Value);
                atm.Credit(_dbContext, _expenditureModel.Cost);

                var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_dbContext, newExpenditure);
                strategy.Debit();
            }
            else
            {
                if (_expenditureModel.Cost != newExpenditure.Cost)
                {
                    var atm = _dbContext.AtmWithdrawModels.Single(x => x.Id == newExpenditure.AtmWithdrawId.Value);

                    if (_expenditureModel.AtmWithdrawId != newExpenditure.AtmWithdrawId)
                    {
                        var oldAtm = _dbContext.AtmWithdrawModels.Single(x => x.Id == _expenditureModel.AtmWithdrawId.Value);
                        oldAtm.Credit(_dbContext, _expenditureModel.Cost);
                        atm.Debit(_dbContext, newExpenditure.Cost);
                    }
                    else
                    {
                        atm.Credit(_dbContext, _expenditureModel.Cost);
                        atm.Debit(_dbContext, newExpenditure.Cost);
                    }
                }
            }
        }
    }
}
