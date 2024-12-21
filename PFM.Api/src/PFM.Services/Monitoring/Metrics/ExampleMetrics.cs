using System.Collections.Generic;
using System.Diagnostics.Metrics;
using PFM.Services.Utils;

namespace PFM.Services.Monitoring.Metrics;

public interface IExampleMetrics
{
    /// <summary>
    /// CountSomething.
    /// </summary>
    /// <param name="operation"></param>
    void CountSomething(string operation);
}

public class ExampleMetrics(IMeterFactory factory) : IExampleMetrics
{    
    public static readonly string MeterName = "PFM.Website.Dashboard";
    private static string FormattedMeterName => MeterName.ToSnakeCase();
    
    private readonly Counter<long> _counter = factory
        .Create(new MeterOptions(MeterName))
        .CreateCounter<long>("count_something","items", "Count something",
            new List<KeyValuePair<string, object>>()
            {
                { new("context", FormattedMeterName) }
            });
    
    private const string OperationTag = "operation";
    
    public void CountSomething(string operation)
    {
        _counter?.Add(1, tags: new []
        {
            new KeyValuePair<string, object>(OperationTag, operation.ToSnakeCase())
        });
    }
}