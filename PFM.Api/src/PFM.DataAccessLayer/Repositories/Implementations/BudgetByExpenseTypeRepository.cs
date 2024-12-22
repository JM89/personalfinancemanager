using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class BudgetByExpenseTypeRepository(PFMContext db)
        : BaseRepository<BudgetByExpenseType>(db), IBudgetByExpenseTypeRepository;
}
