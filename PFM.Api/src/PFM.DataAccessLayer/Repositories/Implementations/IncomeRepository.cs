using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class IncomeRepository(PFMContext db) : BaseRepository<Income>(db), IIncomeRepository;
}
