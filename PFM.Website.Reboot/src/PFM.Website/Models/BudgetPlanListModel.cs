using System;
namespace PFM.Website.Models
{
	public class BudgetPlanListModel
	{
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public bool CanBeChanged => !StartDate.HasValue;
    }
}

