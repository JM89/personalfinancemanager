using System.Collections.Generic;
using PersonalFinanceManager.DTOs.AtmWithdraw;
using PersonalFinanceManager.DTOs.Expense;
using PersonalFinanceManager.DTOs.Income;

namespace PersonalFinanceManager.DTOs.AccountManagement
{
    public class ImportMovementDetails
    {
        public int AccountId { get; set; }

        public IList<ExpenseDetails> Expenses { get; set; }

        public IList<IncomeDetails> Incomes { get; set; }

        public IList<AtmWithdrawDetails> AtmWithdraws { get; set; }
    }
}
