using PersonalFinanceManager.DataAccess.Repositories;
using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.ServicesForTests
{
    public class BankAccountService : IBankAccountService
    {
        public decimal GetAccountAmount(int id)
        {
            using (var dbContext = new DataAccess.ApplicationDbContext())
            {
                var bankAccountRepository = new BankAccountRepository(dbContext);
                var account = bankAccountRepository.GetById(id);
                return account.CurrentBalance;
            }
        }
    }
}
