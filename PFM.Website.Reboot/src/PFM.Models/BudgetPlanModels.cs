using System.ComponentModel.DataAnnotations;

namespace PFM.Models;

public class BudgetPlanListModel
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DateTime? PlannedStartDate { get; set; }

    public bool CanBeChanged => !StartDate.HasValue;

    public string DisplayedStartDate => this.StartDate?.ToString("yyyy-MM-dd") ?? "";

    public string DisplayedEndDate => this.EndDate?.ToString("yyyy-MM-dd") ?? "";
}

public class BudgetPlanEditModel
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public DateTime? StartDate { get; set; } = null;

    public DateTime? EndDate { get; set; } = null;

    public DateTime? PlannedStartDate { get; set; } = null;

    public decimal ExpectedExpenses
    {
        get
        {
            return ExpenseTypes.Any() ? ExpenseTypes.Sum(x => x.ExpectedValue) : 0;
        }
    }

    public decimal ExpectedIncomes { get; set; } = 0.0m;

    public decimal ExpectedSavings { get; set; } = 0.0m;

    public decimal Total => ExpectedIncomes - ExpectedExpenses - ExpectedSavings;

    // Coming from Movement Summary
    public BudgetPlanValueSet PreviousMonth { get; set; } = new ();
    public BudgetPlanValueSet AverageMonth { get; set; } = new ();

    public BudgetPlanEditModel? PreviousBudgetPlan { get; set; } 

    public IEnumerable<BudgetPlanExpenseTypeEditModel> ExpenseTypes { get; set; } = new List<BudgetPlanExpenseTypeEditModel>();
}

public class BudgetPlanExpenseTypeEditModel
{
    public ExpenseTypeModel ExpenseType { get; set; } = new();

    public decimal ExpectedValue { get; set; }
}

public class BudgetPlanValueSet
{
    public decimal Expenses { get; set; }

    public decimal Savings { get; set; }

    public decimal Incomes { get; set; }
}