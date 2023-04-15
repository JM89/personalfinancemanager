using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.Api.Contracts.Shared
{
    public interface ICanBeEdited
    {
        bool CanBeEdited { get; set; }

        string CanBeEditedTooltipResourceName { get; }
    }
}
