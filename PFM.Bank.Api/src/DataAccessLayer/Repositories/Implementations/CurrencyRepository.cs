using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations
{
    public class CurrencyRepository(PFMContext db) : BaseRepository<Currency>(db), ICurrencyRepository;
}
