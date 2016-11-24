using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Utils.Exceptions
{
    public class BusinessException: Exception
    {
        public IDictionary<string,List<string>> ErrorMessages;

        public BusinessException() : base()
        {
            ErrorMessages = new Dictionary<string, List<string>>();
        }

        public BusinessException(string property, string description) : base()
        {
            ErrorMessages = new Dictionary<string, List<string>>();
            AddErrorMessage(property, description);
        }

        public void AddErrorMessage(string property, string description)
        {
            if (!ErrorMessages.ContainsKey(property))
            {
                ErrorMessages.Add(property, new List<string>());
            }
            ErrorMessages[property].Add(description);
        }

        public bool HasError()
        {
            return ErrorMessages.Any();
        }
    }
}
