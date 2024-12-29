using System.ComponentModel.DataAnnotations;

namespace PFM.Website.Models
{
	public class BudgetPlanEditModel
	{
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public DateTime? StartDate { get; set; } = null;

        public DateTime? EndDate { get; set; } = null;

        public DateTime? PlannedStartDate { get; set; } = null;

        public decimal ExpectedExpenses
        {
            get
            {
                return ExpenseTypes.Any() ? ExpenseTypes.Sum(x => x.ExpectedValue) : 0;
            }
        }

        public decimal ExpectedIncomes { get; set; } = 0.0m;

        public decimal ExpectedSavings { get; set; } = 0.0m;

        public decimal Total => ExpectedIncomes - ExpectedExpenses - ExpectedSavings;

        // Coming from Movement Summary
        public BudgetPlanValueSet PreviousMonth { get; set; } = new ();
        public BudgetPlanValueSet AverageMonth { get; set; } = new ();

        public BudgetPlanEditModel? PreviousBudgetPlan { get; set; } 

        public IEnumerable<BudgetPlanExpenseTypeEditModel> ExpenseTypes { get; set; } = new List<BudgetPlanExpenseTypeEditModel>();
    }
}

