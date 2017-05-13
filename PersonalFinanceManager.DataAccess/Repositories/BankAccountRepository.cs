using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class BankAccountRepository : BaseRepository<AccountModel>, IBankAccountRepository
    {
        public BankAccountRepository(ApplicationDbContext db) : base(db)
        {
            
        }

        public decimal GetAccountAmount(int id)
        {
            var entity = GetById(id);
            Refresh(entity);
            return entity.CurrentBalance;
        }
    }
}
