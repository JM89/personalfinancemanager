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
    }
}
