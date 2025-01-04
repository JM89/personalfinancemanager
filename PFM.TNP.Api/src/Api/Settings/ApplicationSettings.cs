using Api.Configurations.Monitoring.Metrics;
using Api.Configurations.Monitoring.Tracing;

namespace Api.Settings;

public class ApplicationSettings
{
    public MetricsOptions MetricsOptions { get; set; } = new ();
    public TracingOptions TracingOptions { get; set; } = new ();
    public AuthOptions AuthOptions { get; set; } = new ();
}