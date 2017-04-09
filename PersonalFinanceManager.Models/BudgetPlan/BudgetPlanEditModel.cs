using PersonalFinanceManager.Models.Helpers;
using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Utils.Helpers;

namespace PersonalFinanceManager.Models.BudgetPlan
{
    public class BudgetPlanEditModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public string DisplayedPlannedStartDate => DateTimeFormatHelper.GetDisplayDateValue(this.PlannedStartDate);

        public bool CanBeChanged => !StartDate.HasValue;

        public string CurrencySymbol { get; set; }

        public decimal ExpenditurePreviousMonthValue { get; set; }

        public string DisplayedExpenditurePreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpenditurePreviousMonthValue, this.CurrencySymbol);

        public decimal? ExpenditureCurrentBudgetPlanValue { get; set; }

        public string DisplayedExpenditureCurrentBudgetPlanValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpenditureCurrentBudgetPlanValue, this.CurrencySymbol);

        public decimal ExpenditureAverageMonthValue { get; set; }

        public string DisplayedExpenditureAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpenditureAverageMonthValue, this.CurrencySymbol);

        public decimal IncomePreviousMonthValue { get; set; }

        public string DisplayedIncomePreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.IncomePreviousMonthValue, this.CurrencySymbol);

        public decimal IncomeAverageMonthValue { get; set; }

        public string DisplayedIncomeAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.IncomeAverageMonthValue, this.CurrencySymbol);

        public decimal TotalPreviousMonthValue { get; set; }

        public string DisplayedTotalPreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.TotalPreviousMonthValue, this.CurrencySymbol);

        public decimal TotalAverageMonthValue { get; set; }

        public string DisplayedTotalAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.TotalAverageMonthValue, this.CurrencySymbol);

        public bool HasCurrentBudgetPlan { get; set; }

        public string BudgetPlanName { get; set; }

        public IList<BudgetPlanExpenditureType> ExpenditureTypes { get; set; }

        [LocalizedDisplayName("BudgetPlanStartDate")]
        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(this.StartDate);

        [LocalizedDisplayName("BudgetPlanEndDate")]
        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(this.EndDate);
    }
}
