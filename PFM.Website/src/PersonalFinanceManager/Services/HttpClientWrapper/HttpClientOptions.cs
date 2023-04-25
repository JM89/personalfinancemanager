using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Services.HttpClientWrapper
{
    public class HttpClientRequestOptions
    {
        public bool AuthenticationTokenRequired { get; set; }

        public HttpClientRequestOptions()
        {
            AuthenticationTokenRequired = true;
        }
    }
}