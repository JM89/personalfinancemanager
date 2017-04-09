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

        public decimal? IncomeCurrentBudgetPlanValue { get; set; }

        public decimal? SavingCurrentBudgetPlanValue { get; set; }

        public decimal IncomePreviousMonthValue { get; set; }

        public decimal IncomeAverageMonthValue { get; set; }

        public decimal SavingPreviousMonthValue { get; set; }

        public decimal SavingAverageMonthValue { get; set; }

        public decimal TotalPreviousMonthValue => this.IncomePreviousMonthValue - this.ExpenditurePreviousMonthValue - this.SavingPreviousMonthValue;

        public decimal TotalAverageMonthValue => this.IncomeAverageMonthValue - this.ExpenditureAverageMonthValue - this.SavingAverageMonthValue;

        public decimal? TotalCurrentBudgetPlanValue => this.IncomeCurrentBudgetPlanValue - this.ExpenditureCurrentBudgetPlanValue - this.SavingCurrentBudgetPlanValue;

        public bool HasCurrentBudgetPlan { get; set; }

        public string BudgetPlanName { get; set; }

        [LocalizedDisplayName("BudgetPlanExpectedIncomes")]
        public decimal ExpectedIncomes { get; set; }

        [LocalizedDisplayName("BudgetPlanExpectedSavings")]
        public decimal ExpectedSavings { get; set; }

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

        public string DisplayedSavingPreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.SavingPreviousMonthValue, this.CurrencySymbol);

        public string DisplayedSavingAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.SavingAverageMonthValue, this.CurrencySymbol);

        public string DisplayedTotalCurrentBudgetPlanValue => DecimalFormatHelper.GetDisplayDecimalValue(this.TotalCurrentBudgetPlanValue, this.CurrencySymbol);

        public string DisplayedTotalPreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.TotalPreviousMonthValue, this.CurrencySymbol);

        public string DisplayedTotalAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.TotalAverageMonthValue, this.CurrencySymbol);

        public string DisplayedSavingCurrentBudgetPlanValue => DecimalFormatHelper.GetDisplayDecimalValue(this.SavingCurrentBudgetPlanValue, this.CurrencySymbol);

        public string DisplayedIncomeCurrentBudgetPlanValue => DecimalFormatHelper.GetDisplayDecimalValue(this.IncomeCurrentBudgetPlanValue, this.CurrencySymbol);
    }
}
