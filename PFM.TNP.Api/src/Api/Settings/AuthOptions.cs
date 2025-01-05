using System;
namespace Api.Settings
{
    public class AuthOptions
    {
        public string Authority { get; set; } = string.Empty;

        public bool RequireHttpsMetadata { get; set; }

        /// <summary>
        /// Disabled when working with docker as the issuer in token is localhost:8080, while the authority is keucloak:8080
        /// </summary>
        public bool ValidateIssuer { get; set; }

        /// <summary>
        /// Disable the authentication/authorization for development purpose. 
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}

