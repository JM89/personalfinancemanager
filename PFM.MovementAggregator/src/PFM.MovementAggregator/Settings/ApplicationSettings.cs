using PFM.MovementAggregator.Monitoring.Metrics;
using PFM.MovementAggregator.Monitoring.Tracing;

namespace PFM.MovementAggregator.Settings
{
	public class ApplicationSettings
    {
		public string DbConnection { get; set; } = string.Empty;
		public MetricsOptions MetricsOptions { get; set; } = new ();
    	public TracingOptions TracingOptions { get; set; } = new ();
	}
}

