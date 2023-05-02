using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class TaxTypeRepository : ITaxTypeRepository
    {
        private readonly PFMContext _db;

        public TaxTypeRepository(PFMContext db)
        {
            this._db = db;
        }

        public DbSet<TaxType> GetList()
        {
            return _db.Set<TaxType>();
        }
    }
}
