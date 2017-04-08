using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Resources;
using PersonalFinanceManager.Utils.Helpers;

namespace PersonalFinanceManager.Models.Dashboard
{
    public class ExpenseSummaryByCategoryModel
    {
        public int CategoryId { get; set; }

        [LocalizedDisplayName("ExpenseSummaryByCategoryCategoryName")]
        public string CategoryName { get; set; }

        public string CategoryColor { get; set; }

        [LocalizedDisplayName("ExpenseSummaryByCategoryCostCurrentMonth")]
        public decimal CostCurrentMonth { get; set; }

        [LocalizedDisplayName("ExpenseSummaryByCategoryCostPlannedMonthly")]
        public decimal CostPlannedMonthly { get; set; }

        [LocalizedDisplayName("ExpenseSummaryByCategoryCostPreviousMonth")]
        public decimal CostPreviousMonth { get; set; }

        [LocalizedDisplayName("ExpenseSummaryByCategoryCostOver12Month")]
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

        public IDictionary<string, List<ExpenditureListModel>> Expenses { get; set; }

        public IDictionary<string, ExpenseSummaryByCategoryAndByMonthModel> ExpensesByMonth { get; set; }

        public ExpenseSummaryByCategoryModel()
        {
            Expenses = new Dictionary<string, List<ExpenditureListModel>>();
            ExpensesByMonth = new Dictionary<string, ExpenseSummaryByCategoryAndByMonthModel>();
        }
    }
}
