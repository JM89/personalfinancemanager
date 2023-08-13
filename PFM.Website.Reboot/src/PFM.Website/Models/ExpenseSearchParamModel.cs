using System;
namespace PFM.Website.Models
{
	public class ExpenseSearchParamModel
	{
        public string UserId { get; set; }

        public int? AccountId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? ExpenseTypeId { get; set; }

        public bool? ShowOnDashboard { get; set; }
    }
}

