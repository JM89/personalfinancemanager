using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class TaxTypeRepository(PFMContext db) : ITaxTypeRepository
    {
        public DbSet<TaxType> GetList()
        {
            return db.Set<TaxType>();
        }
    }
}
