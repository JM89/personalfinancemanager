using System;
namespace PFM.Website.Models
{
	public class BudgetPlanExpenseTypeEditModel
	{
        public ExpenseTypeModel ExpenseType { get; set; }

        public decimal ExpectedValue { get; set; }

        public decimal PreviousMonthValue { get; set; }

        public decimal? CurrentBudgetPlanValue { get; set; }

        public decimal AverageMonthValue { get; set; }

        public string CurrencySymbol { get; set; }
    }
}

