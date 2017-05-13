using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System.Linq;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class SavingRepository : BaseRepository<SavingModel>, ISavingRepository
    {
        public SavingRepository(ApplicationDbContext db) : base(db)
        {

        }

        public int CountSavings()
        {
            return GetList().Count();
        }

        public decimal GetSavingCost(int id)
        {
            var entity = GetById(id);
            Refresh(entity);
            return entity.Amount;
        }
    }
}
