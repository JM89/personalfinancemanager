using PersonalFinanceManager.Entities;

namespace PersonalFinanceManager.DataAccess.Repositories.Interfaces
{
    public interface ISavingRepository : IBaseRepository<SavingModel>
    {
        int CountSavings();

        decimal GetSavingCost(int id);
    }
}
