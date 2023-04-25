using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.Expenditure;

namespace PersonalFinanceManager.Models.Dashboard
{
    public class ExpenseSummaryByCategoryAndByMonthModel
    {
        public decimal CategoryExpenses { get; set; }

        public ExpenseSummaryByCategoryAndByMonthModel(decimal categoryExpenses)
        {
            this.CategoryExpenses = categoryExpenses;
        }
    }
}
