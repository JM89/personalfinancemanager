using System.Collections.Generic;
using PersonalFinanceManager.Api.Contracts.AtmWithdraw;
using PersonalFinanceManager.Api.Contracts.Expense;
using PersonalFinanceManager.Api.Contracts.Income;

namespace PersonalFinanceManager.Api.Contracts.AccountManagement
{
    public class ImportMovementDetails
    {
        public int AccountId { get; set; }

        public IList<ExpenseDetails> Expenses { get; set; }

        public IList<IncomeDetails> Incomes { get; set; }

        public IList<AtmWithdrawDetails> AtmWithdraws { get; set; }
    }
}
