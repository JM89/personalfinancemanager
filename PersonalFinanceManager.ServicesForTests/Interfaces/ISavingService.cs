using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.ServicesForTests.Interfaces
{
    public interface ISavingService
    {
        int CountSavings();

        decimal GetSavingCost(int id);
    }
}
