using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System.Linq;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class HistoricMovementRepository : BaseRepository<HistoricMovement>, IHistoricMovementRepository
    {
        public HistoricMovementRepository(PFMContext db) : base(db)
        {
            
        }
    }
}
