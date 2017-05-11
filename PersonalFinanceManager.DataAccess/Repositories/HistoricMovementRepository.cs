using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using PersonalFinanceManager.Entities.Enumerations;
using System;
namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class HistoricMovementRepository : BaseRepository<HistoricMovementModel>, IHistoricMovementRepository
    {
        public HistoricMovementRepository(ApplicationDbContext db) : base(db)
        {

        }
    }
}
