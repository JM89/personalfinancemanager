using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.Api.Contracts.AccountManagement
{
    public class MovementPropertyDefinition
    {
        public string PropertyName { get; set; }

        public bool HasConfig { get; set; }
    }
}
