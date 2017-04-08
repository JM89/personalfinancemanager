using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Models.Account;
using PersonalFinanceManager.Models.Expenditure;
using PersonalFinanceManager.Utils.Helpers;

namespace PersonalFinanceManager.Models.Dashboard
{
    public class ExpenseSummaryModel
    {
        public AccountEditModel Account { get; set; }

        public List<ExpenseSummaryByCategoryModel> ExpensesByCategory { get; set; }

        public bool HasBudget { get; set; }

        public string BudgetPlanName { get; set; }

        public string LabelCurrentMonth { get; set; }

        public string LabelPreviousMonth { get; set; }

        public string AccountName { get; set; }

        public bool DisplayDashboard { get; set; }

        public string CurrencySymbol { get; set; }

        public decimal TotalExpensesOver12Months { get; set; }

        public decimal CurrentMonthTotalExpense { get; set; }

        public decimal AverageIncomes { get; set; }

        public decimal AverageExpenses { get; set; }

        public decimal AverageSavings { get; set; }

        public string DisplayedCurrentMonthTotalExpense => DecimalFormatHelper.GetDisplayDecimalValue(CurrentMonthTotalExpense, CurrencySymbol);

        public string DisplayedAverageIncomes => DecimalFormatHelper.GetDisplayDecimalValue(AverageIncomes, CurrencySymbol);

        public string DisplayedAverageExpenses => DecimalFormatHelper.GetDisplayDecimalValue(AverageExpenses, CurrencySymbol);

        public string DisplayedAverageSavings => DecimalFormatHelper.GetDisplayDecimalValue(AverageSavings, CurrencySymbol);

        public IDictionary<string, ExpenseSummaryByMonthModel> DetailedExpensesOver12Months { get; set; }

        public IDictionary<string, ExpenseSummaryByMonthModel> DetailedMovementsOver6Months { get; set; }
    }
}
