using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public interface IBankAccountRepository : IBaseRepository<Account>
    {
    }
    
    public class BankAccountRepository(PFMContext db) : BaseRepository<Account>(db), IBankAccountRepository;
}
