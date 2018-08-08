using System.Collections.Generic;
using PFM.DTOs.Expense;

namespace PFM.DTOs.Dashboard
{
    public class ExpenseSummaryByCategory
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string CategoryColor { get; set; }

        public decimal CostCurrentMonth { get; set; }

        public decimal CostPlannedMonthly { get; set; }

        public decimal CostPreviousMonth { get; set; }

        public decimal CostOver12Month { get; set; }

        public decimal AverageCostOver12Months { get; set; }

        public string CurrencySymbol { get; set; }

        public IDictionary<string, List<ExpenseList>> Expenses { get; set; }

        public IDictionary<string, ExpenseSummaryByCategoryAndByMonth> ExpensesByMonth { get; set; }

        public ExpenseSummaryByCategory()
        {
            Expenses = new Dictionary<string, List<ExpenseList>>();
            ExpensesByMonth = new Dictionary<string, ExpenseSummaryByCategoryAndByMonth>();
        }
    }
}
