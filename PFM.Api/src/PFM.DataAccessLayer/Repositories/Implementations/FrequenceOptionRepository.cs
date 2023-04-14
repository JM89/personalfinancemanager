using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class FrequenceOptionRepository : IFrequenceOptionRepository
    {
        private readonly PFMContext _db;

        public FrequenceOptionRepository(PFMContext db)
        {
            this._db = db;
        }

        public DbSet<FrequenceOption> GetList()
        {
            return _db.Set<FrequenceOption>();
        }
    }
}
