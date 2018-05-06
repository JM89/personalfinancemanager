using System.Collections.Generic;
using PersonalFinanceManager.Models.AtmWithdraw;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Models.Income;

namespace PersonalFinanceManager.Models.AccountManagement
{
    public class ImportMovementEditModel
    {
        public int AccountId { get; set; }

        public IList<ExpenditureEditModel> Expenses { get; set; }

        public IList<IncomeEditModel> Incomes { get; set; }

        public IList<AtmWithdrawEditModel> AtmWithdraws { get; set; }
    }
}
