using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations
{
    public class BankAccountRepository : BaseRepository<Account>, IBankAccountRepository
    {
        public BankAccountRepository(PFMContext db) : base(db)
        {
            
        }
    }
}
