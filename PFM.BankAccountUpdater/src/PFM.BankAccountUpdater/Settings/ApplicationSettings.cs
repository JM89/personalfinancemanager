using PFM.BankAccountUpdater.Configurations.Monitoring.Metrics;
using PFM.BankAccountUpdater.Configurations.Monitoring.Tracing;

namespace PFM.BankAccountUpdater.Settings;

public class ApplicationSettings
{
    public MetricsOptions MetricsOptions { get; set; } = new ();
    public TracingOptions TracingOptions { get; set; } = new ();
}