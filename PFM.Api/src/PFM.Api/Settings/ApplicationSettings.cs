using PFM.Services.Monitoring.Metrics;
using PFM.Services.Monitoring.Tracing;

namespace PFM.Api.Settings;

public class ApplicationSettings
{
    public MetricsOptions MetricsOptions { get; set; } = new ();
    public TracingOptions TracingOptions { get; set; } = new ();
}