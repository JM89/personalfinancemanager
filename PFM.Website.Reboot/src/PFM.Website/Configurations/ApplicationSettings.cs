using PFM.Services.Configurations;
using PFM.Services.Monitoring.Metrics;
using PFM.Services.Monitoring.Tracing;

namespace PFM.Website.Configurations
{
	public class ApplicationSettings
	{
		public BankIconSettings BankIconSettings { get; init; } = new();
        public string AwsRegion { get; init; } = "eu-west-2";
        public string AwsEndpointUrl { get; init; } = string.Empty;
        public PfmApiOptions PfmApiOptions { get; init; } = new ();
        public MetricsOptions MetricsOptions { get; init; } = new ();
        public TracingOptions TracingOptions { get; init; } = new ();
	}
}

