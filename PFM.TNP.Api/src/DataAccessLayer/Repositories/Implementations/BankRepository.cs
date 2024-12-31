using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations
{
    public class BankRepository(PFMContext db) : BaseRepository<Bank>(db), IBankRepository;
}
