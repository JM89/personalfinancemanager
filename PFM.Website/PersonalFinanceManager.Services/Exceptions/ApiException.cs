using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Services.Exceptions
{
    public class ApiException: Exception
    {
        public ApiException(string endpoint, string statuscode): base($"{endpoint} failed with {statuscode}")
        {

        }
    }
}
