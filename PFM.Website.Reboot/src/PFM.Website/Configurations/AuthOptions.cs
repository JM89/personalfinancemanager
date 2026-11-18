using System;
namespace PFM.Website.Configurations
{
	public class AuthOptions
	{
        public const string OfflineCookieScheme = "DevOfflineCookie";
        public bool DevOffline { get; set; }
		public string Authority { get; set; } = string.Empty;
        // Browser-facing OIDC URL and token issuer when it differs from the internal authority.
        public string? PublicAuthority { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
	}
}
