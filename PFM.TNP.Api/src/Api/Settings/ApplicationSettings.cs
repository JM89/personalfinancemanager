using Api.Configurations.Monitoring.Metrics;
using Api.Configurations.Monitoring.Tracing;
using DataAccessLayer.Configurations;

namespace Api.Settings;

public class ApplicationSettings
{
    public string ApplicationName { get; set; } = nameof(Api);
    public string ShortDescription { get; set; } = $"This is {nameof(Api)}";
    public MetricsOptions MetricsOptions { get; set; } = new ();
    public TracingOptions TracingOptions { get; set; } = new ();
    public AuthOptions AuthOptions { get; set; } = new ();
    
    public DatabaseOptions DatabaseOptions { get; set; } = new ();
}