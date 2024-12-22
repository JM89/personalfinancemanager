using PFM.Api.Configurations.Monitoring.Metrics;
using PFM.Api.Configurations.Monitoring.Tracing;

namespace PFM.Api.Settings;

public class ApplicationSettings
{
    public MetricsOptions MetricsOptions { get; set; } = new ();
    public TracingOptions TracingOptions { get; set; } = new ();
}