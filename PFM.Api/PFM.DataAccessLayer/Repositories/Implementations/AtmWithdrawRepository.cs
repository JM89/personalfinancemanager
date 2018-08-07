using PFM.DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class AtmWithdrawRepository : BaseRepository<AtmWithdraw>, IAtmWithdrawRepository
    {
        public AtmWithdrawRepository(PFMContext db) : base(db)
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
