using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class AtmWithdrawRepository : BaseRepository<AtmWithdrawModel>, IAtmWithdrawRepository
    {
        public AtmWithdrawRepository(ApplicationDbContext db) : base(db)
        {

        }

        public int CountAtmWithdraws()
        {
            return GetList().Count();
        }

        public decimal GetAtmWithdrawInitialAmount(int id)
        {
            var entity = GetById(id);
            Refresh(entity);
            return entity.InitialAmount;
        }

        public decimal GetAtmWithdrawCurrentAmount(int id)
        {
            var entity = GetById(id);
            Refresh(entity);
            return entity.CurrentAmount;
        }
    }
}
