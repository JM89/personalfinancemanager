using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.ServicesForTests.Interfaces
{
    public interface IAtmWithdrawService
    {
        int CountAtmWithdraws();

        decimal GetAtmWithdrawCost(int id);
    }
}
