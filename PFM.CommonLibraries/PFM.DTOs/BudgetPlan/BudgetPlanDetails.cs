using PFM.Utils.Helpers;
using System;
using System.Collections.Generic;

namespace PFM.DTOs.BudgetPlan
{
    public class BudgetPlanDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public string DisplayedPlannedStartDate => DateTimeFormatHelper.GetDisplayDateValue(this.PlannedStartDate);

        public bool CanBeChanged => !StartDate.HasValue;

        public string CurrencySymbol { get; set; }

        public decimal ExpensePreviousMonthValue { get; set; }

        public decimal? ExpenseCurrentBudgetPlanValue { get; set; }

        public decimal ExpenseAverageMonthValue { get; set; }

        public decimal? IncomeCurrentBudgetPlanValue { get; set; }

        public decimal? SavingCurrentBudgetPlanValue { get; set; }

        public decimal IncomePreviousMonthValue { get; set; }

        public decimal IncomeAverageMonthValue { get; set; }

        public decimal SavingPreviousMonthValue { get; set; }

        public decimal SavingAverageMonthValue { get; set; }

        public decimal TotalPreviousMonthValue => this.IncomePreviousMonthValue - this.ExpensePreviousMonthValue - this.SavingPreviousMonthValue;

        public decimal TotalAverageMonthValue => this.IncomeAverageMonthValue - this.ExpenseAverageMonthValue - this.SavingAverageMonthValue;

        public decimal? TotalCurrentBudgetPlanValue => this.IncomeCurrentBudgetPlanValue - this.ExpenseCurrentBudgetPlanValue - this.SavingCurrentBudgetPlanValue;

        public bool HasCurrentBudgetPlan { get; set; }

        public string BudgetPlanName { get; set; }

        public decimal ExpectedIncomes { get; set; }

        public decimal ExpectedSavings { get; set; }

        public IList<BudgetPlanExpenseType> ExpenseTypes { get; set; }

        public string DisplayedStartDate => DateTimeFormatHelper.GetDisplayDateValue(this.StartDate);

        public string DisplayedEndDate => DateTimeFormatHelper.GetDisplayDateValue(this.EndDate);
        
        public string DisplayedExpenseCurrentBudgetPlanValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpenseCurrentBudgetPlanValue, this.CurrencySymbol);

        public string DisplayedExpensePreviousMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpensePreviousMonthValue, this.CurrencySymbol);

        public string DisplayedExpenseAverageMonthValue => DecimalFormatHelper.GetDisplayDecimalValue(this.ExpenseAverageMonthValue, this.CurrencySymbol);

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
