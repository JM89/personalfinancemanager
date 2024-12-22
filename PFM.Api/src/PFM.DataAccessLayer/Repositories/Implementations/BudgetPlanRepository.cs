using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class BudgetPlanRepository(PFMContext db) : BaseRepository<BudgetPlan>(db), IBudgetPlanRepository;
}
