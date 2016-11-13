using PersonalFinanceManager.Models.ExpenditureType;
using PersonalFinanceManager.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.BudgetPlan
{
    public class BudgetPlanExpenditureType
    {
        [LocalizedDisplayName("BudgetPlanExpenditureType")]
        public ExpenditureTypeListModel ExpenditureType { get; set; }

        [LocalizedDisplayName("BudgetPlanExpectedValue")]
        public decimal ExpectedValue { get; set; }

        [LocalizedDisplayName("BudgetPlanPreviousMonthValue")]
        public decimal PreviousMonthValue { get; set; }

        public string DisplayedPreviousMonthValue
        {
            get
            {
                return this.CurrencySymbol + this.PreviousMonthValue;
            }
        }

        [LocalizedDisplayName("BudgetPlanAverageMonthValue")]
        public decimal AverageMonthValue { get; set; }

        public string DisplayedAverageMonthValue
        {
            get
            {
                return this.CurrencySymbol + this.AverageMonthValue;
            }
        }

        public string CurrencySymbol { get; set; }
    }
}
