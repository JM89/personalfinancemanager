using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class SavingRepository(PFMContext db) : BaseRepository<Saving>(db), ISavingRepository;
}
