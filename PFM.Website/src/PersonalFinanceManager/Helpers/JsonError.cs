using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalFinanceManager.Helpers
{
    public class JsonError
    {
        public string Field;
        public IList<string> ErrorMessages;
    }
}