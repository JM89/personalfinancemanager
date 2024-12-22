using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class ExpenseTypeRepository(PFMContext db) : BaseRepository<ExpenseType>(db), IExpenseTypeRepository;
}
