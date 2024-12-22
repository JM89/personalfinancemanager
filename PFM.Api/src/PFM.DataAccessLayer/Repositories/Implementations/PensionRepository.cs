using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class PensionRepository(PFMContext db) : BaseRepository<Pension>(db), IPensionRepository;
}
