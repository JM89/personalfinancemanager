namespace PFM.Services.Monitoring.Tracing;

public class TracingOptions
{
    public bool Debug { get; init; } = false;
    
    public string ServiceName { get; init; } = string.Empty;
}