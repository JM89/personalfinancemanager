using PFM.Services.Configurations;
using PFM.Services.Monitoring.Metrics;
using PFM.Services.Monitoring.Tracing;

namespace PFM.Website.Configurations
{
	public class ApplicationSettings
	{
		public ExternalServiceSettings ExternalServiceSettings { get; init; } = new();
        public MetricsOptions MetricsOptions { get; init; } = new ();
        public TracingOptions TracingOptions { get; init; } = new ();
	}
}

