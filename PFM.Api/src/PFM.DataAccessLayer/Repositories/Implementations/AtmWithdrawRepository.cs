using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class AtmWithdrawRepository(PFMContext db) : BaseRepository<AtmWithdraw>(db), IAtmWithdrawRepository;
}
