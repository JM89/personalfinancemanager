using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.ServicesForTests.Interfaces
{
    public interface IExpenditureService
    {
        int CountExpenditures();

        decimal GetExpenditureCost(int id);
    }
}
