using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Utils.Exceptions
{
    public class BusinessException: Exception
    {
        public string Property { get; set; }

        public string Description { get; set; }

        public BusinessException(string property, string resource): base()
        {
            Property = property;
            Description = BusinessExceptionMessage.ResourceManager.GetString(resource);
        }

        public BusinessException(string resource) : base()
        {
            Property = "_FORM";
            Description = BusinessExceptionMessage.ResourceManager.GetString(resource);
        }
    }
}
