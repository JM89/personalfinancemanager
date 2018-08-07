using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Repositories.Interfaces
{
    public interface IAtmWithdrawRepository : IBaseRepository<AtmWithdraw>
    {
        int CountAtmWithdraws();

        decimal GetAtmWithdrawInitialAmount(int id);

        decimal GetAtmWithdrawCurrentAmount(int id);
    }
}
