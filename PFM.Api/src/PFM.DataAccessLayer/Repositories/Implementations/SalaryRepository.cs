using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class SalaryRepository(PFMContext db) : BaseRepository<Salary>(db), ISalaryRepository;
}
