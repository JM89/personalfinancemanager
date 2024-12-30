using System;
using System.Collections.Generic;

namespace PFM.Api.Contracts.BudgetPlan
{
    public class BudgetPlanDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public DateTime? PlannedStartDate { get; set; }
        public decimal ExpectedIncomes { get; set; }
        public decimal ExpectedSavings { get; set; }
        public IList<BudgetPlanExpenseType> ExpenseTypes { get; set; }
    }
}
