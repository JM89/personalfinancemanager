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
    public class AtmWithdrawExpenditureStrategy : ExpenditureStrategy
    {
        public AtmWithdrawExpenditureStrategy(IBankAccountRepository bankAccountRepository, IAtmWithdrawRepository atmWithdrawRepository, IIncomeRepository incomeRepository,
            IHistoricMovementRepository historicMovementRepository, ExpenditureModel expenditureModel)
            : base(bankAccountRepository, atmWithdrawRepository, incomeRepository, historicMovementRepository, expenditureModel)
        { }

        public override void Debit()
        {
            var atmWithdrawModel = _atmWithdrawRepository.GetById(_expenditureModel.AtmWithdrawId.Value);
            MovementHelpers.DebitAtmWithdraw(_atmWithdrawRepository, _historicMovementRepository, atmWithdrawModel, _expenditureModel.Cost);
        }

        public override void Credit()
        {
            var atmWithdrawModel = _atmWithdrawRepository.GetById(_expenditureModel.AtmWithdrawId.Value);
            MovementHelpers.CreditAtmWithdraw(_atmWithdrawRepository, _historicMovementRepository, atmWithdrawModel, _expenditureModel.Cost);
        }

        public override void UpdateDebit(ExpenditureModel newExpenditure)
        {
            if (_expenditureModel.PaymentMethodId != newExpenditure.PaymentMethodId)
            {
                var atm = _atmWithdrawRepository.GetById(newExpenditure.AtmWithdrawId.Value);
                MovementHelpers.CreditAtmWithdraw(_atmWithdrawRepository, _historicMovementRepository, atm, _expenditureModel.Cost);

                var strategy = ContextExpenditureStrategy.GetExpenditureStrategy(_bankAccountRepository, _atmWithdrawRepository, _incomeRepository, _historicMovementRepository, newExpenditure);
                strategy.Debit();
            }
            else
            {
                if (_expenditureModel.Cost != newExpenditure.Cost)
                {
                    var atm = _atmWithdrawRepository.GetById(newExpenditure.AtmWithdrawId.Value);

                    if (_expenditureModel.AtmWithdrawId != newExpenditure.AtmWithdrawId)
                    {
                        var oldAtm = _atmWithdrawRepository.GetById(_expenditureModel.AtmWithdrawId.Value);
                        MovementHelpers.CreditAtmWithdraw(_atmWithdrawRepository, _historicMovementRepository, oldAtm, _expenditureModel.Cost);
                        MovementHelpers.DebitAtmWithdraw(_atmWithdrawRepository, _historicMovementRepository, atm, newExpenditure.Cost);
                    }
                    else
                    {
                        MovementHelpers.CreditAtmWithdraw(_atmWithdrawRepository, _historicMovementRepository, atm, _expenditureModel.Cost);
                        MovementHelpers.DebitAtmWithdraw(_atmWithdrawRepository, _historicMovementRepository, atm, newExpenditure.Cost);
                    }
                }
            }
        }
    }
}
