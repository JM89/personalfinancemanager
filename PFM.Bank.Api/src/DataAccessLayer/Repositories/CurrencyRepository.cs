using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public interface ICurrencyRepository : IBaseRepository<Currency>
    {
    }
    
    public class CurrencyRepository(PFMContext db) : BaseRepository<Currency>(db), ICurrencyRepository;
}
