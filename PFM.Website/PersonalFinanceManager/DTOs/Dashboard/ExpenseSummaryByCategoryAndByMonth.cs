using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Api.Contracts.Expense;

namespace PersonalFinanceManager.Api.Contracts.Dashboard
{
    public class ExpenseSummaryByCategoryAndByMonth
    {
        public decimal CategoryExpenses { get; set; }

        public ExpenseSummaryByCategoryAndByMonth(decimal categoryExpenses)
        {
            this.CategoryExpenses = categoryExpenses;
        }
    }
}
