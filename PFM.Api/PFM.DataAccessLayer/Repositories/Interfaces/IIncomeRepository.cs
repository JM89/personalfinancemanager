using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Repositories.Interfaces
{
    public interface IIncomeRepository : IBaseRepository<Income>
    {
        int CountIncomes();

        decimal GetIncomeCost(int id);
    }
}
