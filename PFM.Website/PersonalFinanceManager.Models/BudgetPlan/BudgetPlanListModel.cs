
using PersonalFinanceManager.Models.Resources;
using System;
using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManager.Models.BudgetPlan
{
    public class BudgetPlanListModel
    {
        public int Id { get; set; }

        [LocalizedDisplayName("BudgetPlanName")]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        [LocalizedDisplayName("BudgetPlanStartDate")]
        public string DisplayedStartDate
        {
            get
            {
                return (this.StartDate.HasValue ? this.StartDate.Value.ToString("dd/MM/yyyy") : "");
            }
        }

        public DateTime? EndDate { get; set; }

        [LocalizedDisplayName("BudgetPlanEndDate")]
        public string DisplayedEndDate
        {
            get
            {
                return (this.EndDate.HasValue ? this.EndDate.Value.ToString("dd/MM/yyyy") : "");
            }
        }
    }
}
