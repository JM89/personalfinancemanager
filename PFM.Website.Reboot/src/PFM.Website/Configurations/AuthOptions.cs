using System;
namespace PFM.Website.Configurations
{
	public class AuthOptions
	{
        public const string OfflineCookieScheme = "DevOfflineCookie";
        public bool DevOffline { get; set; }
		public string Authority { get; set; } = string.Empty;
        /// <summary>
        /// Browser-facing OIDC URL and expected token issuer when they differ from
        /// the internal <see cref="Authority"/> used for discovery.
        /// </summary>
        public string? PublicAuthority { get; set; }
        public string ClientId { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
	}
}
