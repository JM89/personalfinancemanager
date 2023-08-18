namespace PFM.Website.Models
{
	public class BudgetPlanEditModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public IEnumerable<BudgetPlanExpenseTypeEditModel> ExpenseTypes { get; set; }
    }
}

