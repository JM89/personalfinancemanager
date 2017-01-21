using PersonalFinanceManager.DataAccess.Repositories;
using System.Linq;
using PersonalFinanceManager.ServicesForTests.Interfaces;

namespace PersonalFinanceManager.ServicesForTests
{
    public class ExpenditureService : IExpenditureService
    {
        public int CountExpenditures()
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var savingRepository = new ExpenditureRepository(dbContext);
                return savingRepository.GetList().ToList().Count();
            }
        }

        public decimal GetExpenditureCost(int id)
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var savingRepository = new ExpenditureRepository(dbContext);
                return savingRepository.GetById(id).Cost;
            }
        }
    }
}
