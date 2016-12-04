using PersonalFinanceManager.DataAccess;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
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
        protected ExpenditureModel _expenditureModel;

        protected IBankAccountRepository _bankAccountRepository;
        protected IAtmWithdrawRepository _atmWithdrawRepository;
        protected IIncomeRepository _incomeRepository;
        protected IHistoricMovementRepository _historicMovementRepository;

        protected ExpenditureStrategy(IBankAccountRepository bankAccountRepository, IAtmWithdrawRepository atmWithdrawRepository,
             IIncomeRepository incomeRepository, IHistoricMovementRepository historicMovementRepository, ExpenditureModel expenditureModel)
        {
            _bankAccountRepository = bankAccountRepository;
            _atmWithdrawRepository = atmWithdrawRepository;
            _historicMovementRepository = historicMovementRepository;
            _incomeRepository = incomeRepository;
            _expenditureModel = expenditureModel;
        }

        public abstract void Debit();

        public abstract void Credit();

        public abstract void UpdateDebit(ExpenditureModel newExpenditure);
    }
}
