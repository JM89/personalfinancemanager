using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{ 
    public interface IBankRepository : IBaseRepository<Bank>
    {
    }
    
    public class BankRepository(PFMContext db) : BaseRepository<Bank>(db), IBankRepository;
}
