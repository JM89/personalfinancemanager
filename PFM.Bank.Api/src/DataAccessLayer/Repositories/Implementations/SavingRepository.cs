using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System.Linq;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class SavingRepository : BaseRepository<Saving>, ISavingRepository
    {
        public SavingRepository(PFMContext db) : base(db)
        {

        }
    }
}
