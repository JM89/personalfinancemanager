using PFM.Services.DTOs.AtmWithdraw;
using PFM.Services.DTOs.Expense;
using PFM.Services.DTOs.Income;
using System.Collections.Generic;

namespace PFM.Services.DTOs.AccountManagement
{
    public class ImportMovementDetails
    {
        public int AccountId { get; set; }

        public IList<ExpenseDetails> Expenses { get; set; }

        public IList<IncomeDetails> Incomes { get; set; }

        public IList<AtmWithdrawDetails> AtmWithdraws { get; set; }
    }
}
