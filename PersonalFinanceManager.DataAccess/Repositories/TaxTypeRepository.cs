using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class TaxTypeRepository : ITaxTypeRepository
    {
        private readonly ApplicationDbContext _db;

        public TaxTypeRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public DbSet<TaxTypeModel> GetList()
        {
            return _db.Set(typeof(TaxTypeModel)).Cast<TaxTypeModel>();
        }
    }
}
