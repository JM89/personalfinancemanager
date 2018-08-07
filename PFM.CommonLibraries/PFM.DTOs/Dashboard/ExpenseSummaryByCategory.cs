using System.Collections.Generic;
using PFM.DTOs.Expense;
using PFM.Utils.Helpers;

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

        public decimal DiffCostPlannedMonthly => CostCurrentMonth - CostPlannedMonthly;

        public decimal DiffCostPreviousMonth => CostCurrentMonth - CostPreviousMonth;

        public decimal DiffAverageCostOver12Months => CostCurrentMonth - AverageCostOver12Months;

        public string DisplayedCostCurrentMonth => DecimalFormatHelper.GetDisplayDecimalValue(CostCurrentMonth, CurrencySymbol);

        public string DisplayedCostPlannedMonthly => DecimalFormatHelper.GetDisplayDecimalValue(CostPlannedMonthly, CurrencySymbol);

        public string DisplayedCostPreviousMonth => DecimalFormatHelper.GetDisplayDecimalValue(CostPreviousMonth, CurrencySymbol);

        public string DisplayedAverageCostOver12Months => DecimalFormatHelper.GetDisplayDecimalValue(AverageCostOver12Months, CurrencySymbol);

        public string DisplayedDiffCostPlannedMonthly => DecimalFormatHelper.GetSignedCurrency(DiffCostPlannedMonthly, CurrencySymbol);

        public string DisplayedDiffCostPreviousMonth => DecimalFormatHelper.GetSignedCurrency(DiffCostPreviousMonth, CurrencySymbol);

        public string DisplayedDiffAverageCostOver12Months => DecimalFormatHelper.GetSignedCurrency(DiffAverageCostOver12Months, CurrencySymbol);

        public IDictionary<string, List<ExpenseList>> Expenses { get; set; }

        public IDictionary<string, ExpenseSummaryByCategoryAndByMonth> ExpensesByMonth { get; set; }

        public ExpenseSummaryByCategory()
        {
            Expenses = new Dictionary<string, List<ExpenseList>>();
            ExpensesByMonth = new Dictionary<string, ExpenseSummaryByCategoryAndByMonth>();
        }
    }
}
