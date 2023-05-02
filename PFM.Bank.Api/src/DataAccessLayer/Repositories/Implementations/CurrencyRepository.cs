using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(PFMContext db) : base(db)
        {

        }
    }
}
