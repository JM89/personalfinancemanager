using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class TaxRepository(PFMContext db) : BaseRepository<Tax>(db), ITaxRepository;
}
