using System;
using System.Collections.Generic;

namespace PersonalFinanceManager.Services.Exceptions
{
    public class ApiException: Exception
    {
        public string HttpStatusCode { get;  }
        public IDictionary<string, List<string>> Errors { get; }

        public ApiException(string endpoint, string statuscode, IDictionary<string, List<string>> errors = null): base($"{endpoint} failed with {statuscode}")
        {
            HttpStatusCode = statuscode;
            Errors = errors;
        }
    }
}
