using PersonalFinanceManager.Models.Income;
using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.BudgetPlan
{
    public class BudgetPlanEditModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }

        public string DisplayedPlannedStartDate {
            get {
                return (this.PlannedStartDate.HasValue ? this.PlannedStartDate.Value.ToString("dd/MM/yyyy") : "");
            }
        }

        public bool CanBeChanged { get
            {
                return !StartDate.HasValue;
            }
        }

        public string CurrencySymbol { get; set; }

        public decimal ExpenditurePreviousMonthValue { get; set; }

        public string DisplayedExpenditurePreviousMonthValue
        {
            get
            {
                return this.CurrencySymbol + this.ExpenditurePreviousMonthValue;
            }
        }
        
        public decimal ExpenditureAverageMonthValue { get; set; }

        public string DisplayedExpenditureAverageMonthValue
        {
            get
            {
                return this.CurrencySymbol + this.ExpenditureAverageMonthValue;
            }
        }

        public decimal IncomePreviousMonthValue { get; set; }

        public string DisplayedIncomePreviousMonthValue
        {
            get
            {
                return this.CurrencySymbol + this.IncomePreviousMonthValue;
            }
        }
        
        public decimal IncomeAverageMonthValue { get; set; }

        public string DisplayedIncomeAverageMonthValue
        {
            get
            {
                return this.CurrencySymbol + this.IncomeAverageMonthValue;
            }
        }

        public decimal TotalPreviousMonthValue { get; set; }

        public string DisplayedTotalPreviousMonthValue
        {
            get
            {
                return this.CurrencySymbol + this.TotalPreviousMonthValue;
            }
        }

        public decimal TotalAverageMonthValue { get; set; }

        public string DisplayedTotalAverageMonthValue
        {
            get
            {
                return this.CurrencySymbol + this.TotalAverageMonthValue;
            }
        }

        public IList<BudgetPlanExpenditureType> ExpenditureTypes { get; set; }

        [LocalizedDisplayName("BudgetPlanStartDate")]
        public string DisplayedStartDate
        {
            get
            {
                return (this.StartDate.HasValue ? this.StartDate.Value.ToString("dd/MM/yyyy") : "");
            }
        }

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
