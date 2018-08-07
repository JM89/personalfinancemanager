using System.Collections.Generic;
using PFM.DTOs.Account;
using PFM.Utils.Helpers;

namespace PFM.DTOs.Dashboard
{
    public class ExpenseSummary
    {
        public AccountDetails Account { get; set; }

        public List<ExpenseSummaryByCategory> ExpensesByCategory { get; set; }

        public bool HasCurrentBudgetPlan { get; set; }

        public bool HasExpenses { get; set; }

        public bool HasCategories { get; set; }

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

        public IDictionary<string, ExpenseSummaryByMonth> DetailedExpensesOver12Months { get; set; }

        public IDictionary<string, ExpenseSummaryByMonth> DetailedMovementsOver6Months { get; set; }
    }
}
