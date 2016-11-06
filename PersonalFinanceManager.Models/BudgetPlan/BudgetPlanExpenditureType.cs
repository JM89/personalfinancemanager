using PersonalFinanceManager.Models.ExpenditureType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.BudgetPlan
{
    public class BudgetPlanExpenditureType
    {
        public ExpenditureTypeListModel ExpenditureType { get; set; }

        public decimal ExpectedValue { get; set; }

        public decimal PeriodicOutcomeValue { get; set; }

        public decimal PreviousMonthValue { get; set; }

        public decimal AverageMonthValue { get; set; }
    }
}
