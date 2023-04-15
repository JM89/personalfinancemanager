using System;

namespace PersonalFinanceManager.Api.Contracts.BudgetPlan
{
    public class BudgetPlanList
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public string DisplayedStartDate
        {
            get
            {
                return (this.StartDate.HasValue ? this.StartDate.Value.ToString("dd/MM/yyyy") : "");
            }
        }

        public DateTime? EndDate { get; set; }

        public string DisplayedEndDate
        {
            get
            {
                return (this.EndDate.HasValue ? this.EndDate.Value.ToString("dd/MM/yyyy") : "");
            }
        }
    }
}
