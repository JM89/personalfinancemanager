using System;
namespace PFM.Website.Configurations
{
	public class ApplicationSettings
	{
		public bool UseRemoteStorageForBankIcons { get; set; }
		public string BankIconLocation { get; set; }
        public string AwsRegion { get; set; }
        public string AwsEndpointUrl { get; set; }
    }
}

