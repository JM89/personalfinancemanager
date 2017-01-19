using PersonalFinanceManager.DataAccess.Repositories;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
