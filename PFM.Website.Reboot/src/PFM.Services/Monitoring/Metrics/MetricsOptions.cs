namespace PFM.Services.Monitoring.Metrics;

public class MetricsOptions
{
    public bool Debug { get; init; } = false;
    
    public string ServiceName { get; init; } = string.Empty;
}