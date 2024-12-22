using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations
{
    public class BankAccountRepository(PFMContext db) : BaseRepository<Account>(db), IBankAccountRepository;
}
