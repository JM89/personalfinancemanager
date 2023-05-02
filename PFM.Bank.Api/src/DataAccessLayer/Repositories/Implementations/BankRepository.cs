using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations
{
    public class BankRepository : BaseRepository<Bank>, IBankRepository
    {
        public BankRepository(PFMContext db) : base(db)
        {

        }
    }
}
