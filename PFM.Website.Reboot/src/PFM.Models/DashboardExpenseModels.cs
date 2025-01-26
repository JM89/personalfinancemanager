namespace PFM.Models;

public class DashboardExpenseOvertimeModel(IDictionary<string, ExpenseOvertimeModel> aggregates)
{
    /// <summary>
    /// Key: MonthYear, Value: Actual sum, Expected sum (budget plan)
    /// </summary>
    public IDictionary<string, ExpenseOvertimeModel> Aggregates { get; } = aggregates;
}

public class ExpenseOvertimeModel
{
    public decimal Actual { get; set; }

    /// <summary>
    /// For budget implementation
    /// </summary>
    public decimal Expected { get; set; }
}

public class DashboardExpenseTypeDiffsModel(PagedModel<ExpenseTypeDiffsModel> pagedList)
{
    public PagedModel<ExpenseTypeDiffsModel> PagedList { get; } = pagedList;
}

public class ExpenseTypeDiffsModel
{
    public string ExpenseTypeName { get; set; } = string.Empty;

    public decimal Actual { get; set; }

    public decimal Expected { get; set; }

    public decimal ExpectedDiff => Actual - Expected;

    public decimal PreviousMonth { get; set; }

    public decimal PreviousMonthDiff => Actual - PreviousMonth;

    public decimal Average { get; set; }

    public decimal AverageDiff => Actual - Average;
}

public class DashboardExpenseTypeOvertimeModel(IDictionary<string, decimal> aggregates)
{
    /// <summary>
    /// Key: MonthYear, Value: Sum Amounts
    /// </summary>
    public IDictionary<string, decimal> Aggregates = aggregates;
}

public class DashboardExpenseTypeSummaryModel(IDictionary<string, decimal> aggregates)
{
    /// <summary>
    /// Key: Expense Type Name, Value: Actual sum, Expected sum (budget plan)
    /// </summary>
    public IDictionary<string, decimal> Aggregates { get; } = aggregates;
}

public class DashboardMovementTypeOvertimeModel(IDictionary<string, MovementTypeOvertimeModel> aggregates)
{
    /// <summary>
    /// Key: MonthYear, Value: Expense sum, Incomes sum, Savings sum
    /// </summary>
    public IDictionary<string, MovementTypeOvertimeModel> Aggregates { get; } = aggregates;
}

public class MovementTypeOvertimeModel
{
    public decimal ExpensesAmount { get; set; }

    public decimal IncomesAmount { get; set; }

    public decimal SavingsAmount { get; set; }
}

public class DashboardMovementTypeSummaryModel(
    decimal totalExpensesCurrentMonth,
    decimal averageExpensesOver12Months,
    decimal averageIncomesOver12Months,
    decimal averageSavingsOver12Months)
{
    public decimal TotalExpensesCurrentMonth { get; set; } = totalExpensesCurrentMonth;

    public decimal AverageExpensesOver12Months { get; set; } = averageExpensesOver12Months;

    public decimal AverageIncomesOver12Months { get; set; } = averageIncomesOver12Months;

    public decimal AverageSavingsOver12Months { get; set; } = averageSavingsOver12Months;
}