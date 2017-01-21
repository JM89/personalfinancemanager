using PersonalFinanceManager.DataAccess.Repositories;
using System.Linq;
using PersonalFinanceManager.ServicesForTests.Interfaces;

namespace PersonalFinanceManager.ServicesForTests
{
    public class IncomeService : IIncomeService
    {
        public int CountIncomes()
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var incomeRepository = new IncomeRepository(dbContext);
                return incomeRepository.GetList().ToList().Count();
            }
        }

        public decimal GetIncomeCost(int id)
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var incomeRepository = new IncomeRepository(dbContext);
                return incomeRepository.GetById(id).Cost;
            }
        }
    }
}
