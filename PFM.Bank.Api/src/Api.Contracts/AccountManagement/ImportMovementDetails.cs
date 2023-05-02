using PFM.Api.Contracts.AtmWithdraw;
using PFM.Api.Contracts.Expense;
using PFM.Api.Contracts.Income;
using System.Collections.Generic;

namespace PFM.Api.Contracts.AccountManagement
{
    public class ImportMovementDetails
    {
        public int AccountId { get; set; }

        public IList<ExpenseDetails> Expenses { get; set; }

        public IList<IncomeDetails> Incomes { get; set; }

        public IList<AtmWithdrawDetails> AtmWithdraws { get; set; }
    }
}
