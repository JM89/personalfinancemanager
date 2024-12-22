using Services.Monitoring.Metrics;
using Services.Monitoring.Tracing;

namespace Api.Settings;

public class ApplicationSettings
{
    public MetricsOptions MetricsOptions { get; set; } = new ();
    public TracingOptions TracingOptions { get; set; } = new ();
}