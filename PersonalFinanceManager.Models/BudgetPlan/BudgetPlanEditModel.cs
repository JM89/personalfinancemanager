using PersonalFinanceManager.Models.Income;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Models.BudgetPlan
{
    public class BudgetPlanEditModel
    {
        public string BudgetName { get; set; }

        public DateTime StartDate { get; set; }

        public IList<BudgetPlanExpenditureType> ExpenditureTypes { get; set; }

        public IList<IncomeListModel> Incomes { get; set; }
    }
}
