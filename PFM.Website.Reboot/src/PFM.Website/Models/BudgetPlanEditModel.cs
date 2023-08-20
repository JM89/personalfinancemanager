using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
	public class BudgetPlanEditModel
	{
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public decimal ExpectedIncomes { get; set; }

        public decimal ExpectedSavings { get; set; }

        // Coming from Movement Summary
        public BudgetPlanValueSet PreviousMonth { get; set; } = new BudgetPlanValueSet();
        public BudgetPlanValueSet AverageMonth { get; set; } = new BudgetPlanValueSet();

        public BudgetPlanEditModel? PreviousBudgetPlan { get; set; }

        public string CurrencySymbol { get; set; }

        public IEnumerable<BudgetPlanExpenseTypeEditModel> ExpenseTypes { get; set; }
    }
}

