using PersonalFinanceManager.Entities;

namespace PersonalFinanceManager.DataAccess.Repositories.Interfaces
{
    public interface IHistoricMovementRepository : IBaseRepository<HistoricMovementModel>
    {
        int CountMovements();
    }
}
