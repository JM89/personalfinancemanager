using DataAccessLayer.Entities;

namespace DataAccessLayer.Repositories
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
    }
    
    public class CountryRepository(PFMContext db) : BaseRepository<Country>(db), ICountryRepository;
}
