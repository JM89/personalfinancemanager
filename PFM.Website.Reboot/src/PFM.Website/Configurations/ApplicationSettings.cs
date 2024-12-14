using System;
namespace PFM.Website.Configurations
{
	public class ApplicationSettings
	{
		public bool UseRemoteStorageForBankIcons { get; init; }
		public string BankIconLocation { get; init; } = string.Empty;
        public string AwsRegion { get; init; } = "eu-west-2";
        public string AwsEndpointUrl { get; init; } = string.Empty;
        public PfmApiOptions PfmApiOptions { get; init; } = new ();
        
        public MetricsOptions MetricsOptions { get; init; } = new ();
        
        public TracingOptions TracingOptions { get; init; } = new ();
	}

	public class PfmApiOptions
	{
		public bool Enabled { get; init; } = false;
		public string EndpointUrl { get; init; } = string.Empty;
	}

	public class MetricsOptions
	{
		public bool Debug { get; init; } = false;
	}
	
	public class TracingOptions
	{
		public bool Debug { get; init; } = false;
	}
}

