using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories.Interfaces
{
    public interface IAtmWithdrawRepository : IBaseRepository<AtmWithdrawModel>
    {
        int CountAtmWithdraws();

        decimal GetAtmWithdrawInitialAmount(int id);

        decimal GetAtmWithdrawCurrentAmount(int id);
    }
}
