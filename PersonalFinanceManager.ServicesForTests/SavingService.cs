using PersonalFinanceManager.DataAccess.Repositories;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.ServicesForTests
{
    public class SavingService : ISavingService
    {
        public int CountSavings()
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var savingRepository = new SavingRepository(dbContext);
                return savingRepository.GetList().ToList().Count();
            }
        }

        public decimal GetSavingCost(int id)
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var savingRepository = new SavingRepository(dbContext);
                return savingRepository.GetById(id).Amount;
            }
        }
    }
}
