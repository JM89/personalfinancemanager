using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class FrequenceOptionRepository(PFMContext db) : IFrequenceOptionRepository
    {
        public DbSet<FrequenceOption> GetList()
        {
            return db.Set<FrequenceOption>();
        }
    }
}
