using System;

namespace PFM.Authentication.Api.Models
{
    public class ValidateApiKeyRequest
    {
        public Guid AppId { get; set; }

        public string ApiKey { get; set; }
    }
}
