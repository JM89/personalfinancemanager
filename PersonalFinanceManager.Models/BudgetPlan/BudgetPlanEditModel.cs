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

        public decimal? ExpenditureCurrentBudgetPlanValue { get; set; }

        public decimal ExpenditureAverageMonthValue { get; set; }

        public decimal IncomePreviousMonthValue { get; set; }

        public decimal IncomeAverageMonthValue { get; set; }

        public decimal TotalPreviousMonthValue => this.IncomePreviousMonthValue - this.ExpenditurePreviousMonthValue;

        public decimal TotalAverageMonthValue => this.IncomeAverageMonthValue - this.ExpenditureAverageMonthValue;

        public bool HasCurrentBudgetPlan { get; set; }

        public string BudgetPlanName { get; set; }

        public IList<BudgetPlanExpenditureType> ExpenditureTypes { get; set; }

        [LocalizedDisplayName("BudgetPlanStartDate")]
        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(this.StartDate);

        [LocalizedDisplayName("BudgetPlanEndDate")]
        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(this.EndDate);
        
        public string DisplayedExpenditureCurrentBudgetPlanValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpenditureCurrentBudgetPlanValue, this.CurrencySymbol);

        public string DisplayedExpenditurePreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpenditurePreviousMonthValue, this.CurrencySymbol);

        public string DisplayedExpenditureAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpenditureAverageMonthValue, this.CurrencySymbol);

        public string DisplayedIncomePreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.IncomePreviousMonthValue, this.CurrencySymbol);

        public string DisplayedIncomeAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.IncomeAverageMonthValue, this.CurrencySymbol);

        public string DisplayedTotalPreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.TotalPreviousMonthValue, this.CurrencySymbol);

        public string DisplayedTotalAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.TotalAverageMonthValue, this.CurrencySymbol);
    }
}
