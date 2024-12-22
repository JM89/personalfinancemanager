using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class SalaryDeductionRepository(PFMContext db)
        : BaseRepository<SalaryDeduction>(db), ISalaryDeductionRepository;
}
