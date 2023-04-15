using PersonalFinanceManager.Api.Contracts.ExpenseType;

namespace PersonalFinanceManager.Api.Contracts.BudgetPlan
{
    public class BudgetPlanExpenseType
    {
        public ExpenseTypeList ExpenseType { get; set; }

        public decimal ExpectedValue { get; set; }

        public decimal PreviousMonthValue { get; set; }

        public decimal? CurrentBudgetPlanValue { get; set; }

        public decimal AverageMonthValue { get; set; }

        public string CurrencySymbol { get; set; }
    }
}
