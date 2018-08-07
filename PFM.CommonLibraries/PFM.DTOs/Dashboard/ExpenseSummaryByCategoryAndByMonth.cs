using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFM.DTOs.Expense;

namespace PFM.DTOs.Dashboard
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
