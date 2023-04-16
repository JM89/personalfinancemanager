using System;

namespace PFM.Authentication.Api.Models
{
    public class RegisterAppRequest
    {
        public Guid AppId { get; set; }

        public string AppName { get; set; }
    }
}
