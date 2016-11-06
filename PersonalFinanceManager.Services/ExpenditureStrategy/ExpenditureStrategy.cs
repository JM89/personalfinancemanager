using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.ExpenditureStrategy
{
    public abstract class ExpenditureStrategy
    {
        protected ApplicationDbContext _dbContext;
        protected ExpenditureModel _expenditureModel;

        protected ExpenditureStrategy(ApplicationDbContext dbContext, ExpenditureModel expenditureModel)
        {
            _dbContext = dbContext;
            _expenditureModel = expenditureModel;
        }

        public abstract void Debit();

        public abstract void Credit();

        public abstract void UpdateDebit(ExpenditureModel newExpenditure);
    }
}
