using PFM.DTOs.ExpenseType;
using PFM.Utils.Helpers;

namespace PFM.DTOs.BudgetPlan
{
    public class BudgetPlanExpenseType
    {
        public ExpenseTypeList ExpenseType { get; set; }

        public decimal ExpectedValue { get; set; }

        public decimal PreviousMonthValue { get; set; }

        public decimal? CurrentBudgetPlanValue { get; set; }

        public decimal AverageMonthValue { get; set; }

        public string DisplayedPreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.PreviousMonthValue, this.CurrencySymbol);

        public string DisplayedCurrentBudgetPlanValue => DecimalFormatHelper.GetDisplayDecimalValue(this.CurrentBudgetPlanValue, this.CurrencySymbol);

        public string DisplayedAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.AverageMonthValue, this.CurrencySymbol);

        public string CurrencySymbol { get; set; }
    }
}
