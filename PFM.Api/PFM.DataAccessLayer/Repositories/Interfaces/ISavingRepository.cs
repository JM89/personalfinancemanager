using PFM.DataAccessLayer.Entities;

namespace PFM.DataAccessLayer.Repositories.Interfaces
{
    public interface ISavingRepository : IBaseRepository<Saving>
    {
        int CountSavings();

        decimal GetSavingCost(int id);
    }
}
