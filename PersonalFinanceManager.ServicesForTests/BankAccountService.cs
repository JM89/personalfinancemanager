using PersonalFinanceManager.DataAccess.Repositories;
using PersonalFinanceManager.ServicesForTests.Interfaces;

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
