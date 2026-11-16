using System;
namespace PFM.Website.Configurations
{
	public class AuthOptions
	{
        public const string OfflineCookieScheme = "DevOfflineCookie";
        public bool DevOffline { get; set; }
		public string Authority { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
	}
}
