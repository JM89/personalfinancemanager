using System.Diagnostics.Metrics;
using PFM.Utils;

namespace PFM.Website.Monitoring.Metrics;

public interface IDashboardMetrics
{
    /// <summary>
    /// Increment the counter for an operation and optional expense type.
    /// </summary>
    /// <param name="operation"></param>
    /// <param name="expenseType"></param>
    void RecordClick(string operation, string? expenseType = null);
}

public class DashboardMetrics(IMeterFactory factory) : IDashboardMetrics
{    
    public static readonly string MeterName = "PFM.Website.Dashboard";
    private static string FormattedMeterName => MeterName.ToSnakeCase();
    
    private readonly Counter<long>? _dashboardActivityCounter = factory
        .Create(new MeterOptions(MeterName))
        .CreateCounter<long>("dashboard.click_activity","items", "Count clicks on the dashboard",
            new List<KeyValuePair<string, object?>>()
            {
                { new("context", FormattedMeterName) }
            });
    
    private const string OperationTag = "operation";
    private const string ExpenseTypeTag = "expense_type";
    
    public void RecordClick(string operation, string? expenseType = null)
    {
        _dashboardActivityCounter?.Add(1, tags: new []
        {
            new KeyValuePair<string, object?>(OperationTag, operation.ToSnakeCase()),
            new KeyValuePair<string, object?>(ExpenseTypeTag, expenseType?.ToSnakeCase())
        });
    }
}