using System;
using System.Collections.Generic;

namespace PFM.Services.DTOs.BudgetPlan
{
    public class BudgetPlanDetails
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }

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

        public bool HasCurrentBudgetPlan { get; set; }

        public string BudgetPlanName { get; set; }

        public decimal ExpectedIncomes { get; set; }

        public decimal ExpectedSavings { get; set; }

        public IList<BudgetPlanExpenseType> ExpenseTypes { get; set; }
    }
}
