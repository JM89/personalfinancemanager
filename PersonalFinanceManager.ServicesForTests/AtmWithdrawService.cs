using PersonalFinanceManager.DataAccess.Repositories;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.ServicesForTests.Interfaces;

namespace PersonalFinanceManager.ServicesForTests
{
    public class AtmWithdrawService : IAtmWithdrawService
    {
        public int CountAtmWithdraws()
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var atmWithdrawRepository = new AtmWithdrawRepository(dbContext);
                return atmWithdrawRepository.GetList().ToList().Count();
            }
        }

        public decimal GetAtmWithdrawInitialAmount(int id)
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var atmWithdrawRepository = new AtmWithdrawRepository(dbContext);
                return atmWithdrawRepository.GetById(id).InitialAmount;
            }
        }

        public decimal GetAtmWithdrawCurrentAmount(int id)
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var atmWithdrawRepository = new AtmWithdrawRepository(dbContext);
                return atmWithdrawRepository.GetById(id).CurrentAmount;
            }
        }
    }
}
