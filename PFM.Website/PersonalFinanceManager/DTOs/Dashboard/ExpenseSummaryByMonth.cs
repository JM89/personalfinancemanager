using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.DTOs.Expense;

namespace PersonalFinanceManager.DTOs.Dashboard
{
    public class ExpenseSummaryByMonth
    {
        public decimal? IncomeValue { get; set; }

        public decimal? ExpenseValue { get; set; }

        public decimal? ExpenseExpectedValue { get; set; }

        public decimal? SavingValue { get; set; }
    }
}
