using PersonalFinanceManager.DataAccess.Repositories;
using System.Linq;
using PersonalFinanceManager.ServicesForTests.Interfaces;

namespace PersonalFinanceManager.ServicesForTests
{
    public class HistoricMovementService : IHistoricMovementService
    {
        public int CountMovements()
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var historicMovementRepository = new HistoricMovementRepository(dbContext);
                return historicMovementRepository.GetList().ToList().Count();
            }
        }
    }
}
