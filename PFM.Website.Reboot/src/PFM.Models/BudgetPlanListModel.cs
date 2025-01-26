namespace PFM.Models
{
	public class BudgetPlanListModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public bool CanBeChanged => !StartDate.HasValue;

        public string DisplayedStartDate
        {
            get
            {
                return this.StartDate?.ToString("yyyy-MM-dd") ?? "";
            }
        }

        public string DisplayedEndDate
        {
            get
            {
                return this.EndDate?.ToString("yyyy-MM-dd") ?? "";
            }
        }
    }
}

