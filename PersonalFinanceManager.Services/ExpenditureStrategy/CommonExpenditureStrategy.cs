//using PersonalFinanceManager.DataAccess;
//using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
//using PersonalFinanceManager.Entities;
//using PersonalFinanceManager.Entities.Enumerations;
//using PersonalFinanceManager.Services.Helpers;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace PersonalFinanceManager.Services.ExpenditureStrategy
//{
//    public class CommonExpenditureStrategy : ExpenditureStrategy
//    {
//        public CommonExpenditureStrategy(IBankAccountRepository bankAccountRepository, IAtmWithdrawRepository atmWithdrawRepository, IIncomeRepository incomeRepository,
//            IHistoricMovementRepository historicMovementRepository, ExpenditureModel expenditureModel)
//            : base(bankAccountRepository, atmWithdrawRepository, incomeRepository, historicMovementRepository, expenditureModel)
//        { }

//        public override void Debit()
//        {
//            var account = _bankAccountRepository.GetById(_expenditureModel.AccountId);
//            account.CurrentBalance -= _expenditureModel.Cost;
//            _bankAccountRepository.Update(account);
//        }

//        public override void Credit()
//        {
//            var account = _bankAccountRepository.GetById(_expenditureModel.AccountId);
//            account.CurrentBalance += _expenditureModel.Cost;
//            _bankAccountRepository.Update(account);
//        }

//        public override void UpdateDebit(ExpenditureModel newExpenditure)
//        {
//            if (_expenditureModel.PaymentMethodId != newExpenditure.PaymentMethodId)
//            {
//                var account = _bankAccountRepository.GetById(_expenditureModel.AccountId);
//                account.CurrentBalance += _expenditureModel.Cost;
//                _bankAccountRepository.Update(account);

//                var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_bankAccountRepository, _atmWithdrawRepository, _incomeRepository, _historicMovementRepository, newExpenditure);
//                strategy.Debit();
//            }
//            else
//            {
//                if (_expenditureModel.Cost != newExpenditure.Cost)
//                {
//                    var account = _bankAccountRepository.GetById(_expenditureModel.AccountId);
//                    account.CurrentBalance += _expenditureModel.Cost;
//                    _bankAccountRepository.Update(account);
//                    account.CurrentBalance -= newExpenditure.Cost;
//                    _bankAccountRepository.Update(account);
//                }
//            }
//        }
//    }
//}
