using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.Expenditure;

namespace PersonalFinanceManager.Models.Dashboard
{
    public class ExpenseSummaryModel
    {
        public List<ExpenseSummaryByCategoryModel> ExpensesByCategory { get; set; }

        public bool HasBudget { get; set; }

        public string BudgetPlanName { get; set; }

        public string LabelCurrentMonth { get; set; }

        public string LabelPreviousMonth { get; set; }

        public string AccountName { get; set; }

        public bool DisplayDashboard { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal TotalExpensesOver12Months { get; set; }
    }
}
