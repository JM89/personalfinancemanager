using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System.Linq;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class HistoricMovementRepository : BaseRepository<HistoricMovementModel>, IHistoricMovementRepository
    {
        public HistoricMovementRepository(ApplicationDbContext db) : base(db)
        {
            
        }

        public int CountMovements()
        {
            return GetList().Count();
        }
    }
}
