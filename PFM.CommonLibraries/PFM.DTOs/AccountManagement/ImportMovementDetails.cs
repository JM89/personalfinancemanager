using System.Collections.Generic;
using PFM.DTOs.AtmWithdraw;
using PFM.DTOs.Expense;
using PFM.DTOs.Income;

namespace PFM.DTOs.AccountManagement
{
    public class ImportMovementDetails
    {
        public int AccountId { get; set; }

        public IList<ExpenseDetails> Expenses { get; set; }

        public IList<IncomeDetails> Incomes { get; set; }

        public IList<AtmWithdrawDetails> AtmWithdraws { get; set; }
    }
}
