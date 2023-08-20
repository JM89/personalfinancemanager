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

        public decimal ExpectedExpenses
        {
            get
            {
                return ExpenseTypes.Any() ? ExpenseTypes.Sum(x => x.ExpectedValue) : 0;
            }
        }

        public decimal ExpectedIncomes { get; set; }

        public decimal ExpectedSavings { get; set; }

        public decimal Total
        {
            get
            {
                return ExpectedIncomes - ExpectedExpenses - ExpectedSavings; 
            }
        }

        // Coming from Movement Summary
        public BudgetPlanValueSet PreviousMonth { get; set; } = new BudgetPlanValueSet();
        public BudgetPlanValueSet AverageMonth { get; set; } = new BudgetPlanValueSet();

        public BudgetPlanEditModel? PreviousBudgetPlan { get; set; }

        public IEnumerable<BudgetPlanExpenseTypeEditModel> ExpenseTypes { get; set; }
    }
}

