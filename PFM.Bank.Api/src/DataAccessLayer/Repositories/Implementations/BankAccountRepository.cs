using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class BankAccountRepository : BaseRepository<Account>, IBankAccountRepository
    {
        public BankAccountRepository(PFMContext db) : base(db)
        {
            
        }
    }
}
