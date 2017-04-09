using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Utils.Helpers;

namespace PersonalFinanceManager.Models.BudgetPlan
{
    public class BudgetPlanExpenditureType
    {
        [LocalizedDisplayName("BudgetPlanExpenditureType")]
        public ExpenditureTypeListModel ExpenditureType { get; set; }

        [LocalizedDisplayName("BudgetPlanExpectedValue")]
        public decimal ExpectedValue { get; set; }

        [LocalizedDisplayName("BudgetPlanPreviousMonthValue")]
        public decimal PreviousMonthValue { get; set; }

        public decimal? CurrentBudgetPlanValue { get; set; }

        [LocalizedDisplayName("BudgetPlanAverageMonthValue")]
        public decimal AverageMonthValue { get; set; }

        public string DisplayedPreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.PreviousMonthValue, this.CurrencySymbol);

        public string DisplayedCurrentBudgetPlanValue => DecimalFormatHelper.GetDisplayDecimalValue(this.CurrentBudgetPlanValue, this.CurrencySymbol);

        public string DisplayedAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.AverageMonthValue, this.CurrencySymbol);

        public string CurrencySymbol { get; set; }
    }
}
