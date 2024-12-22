using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;

namespace DataAccessLayer.Repositories.Implementations
{
    public class CountryRepository(PFMContext db) : BaseRepository<Country>(db), ICountryRepository;
}
