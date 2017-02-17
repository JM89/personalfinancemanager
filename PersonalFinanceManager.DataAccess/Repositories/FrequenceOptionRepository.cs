using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System.Data.Entity;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class FrequenceOptionRepository : IFrequenceOptionRepository
    {
        private readonly ApplicationDbContext _db;

        public FrequenceOptionRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public DbSet<FrequenceOptionModel> GetList()
        {
            return _db.Set(typeof(FrequenceOptionModel)).Cast<FrequenceOptionModel>();
        }
    }
}
