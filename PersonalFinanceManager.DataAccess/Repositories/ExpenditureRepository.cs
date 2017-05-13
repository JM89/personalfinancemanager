using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using PersonalFinanceManager.Entities.SearchParameters;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class ExpenditureRepository : BaseRepository<ExpenditureModel>, IExpenditureRepository
    {
        public ExpenditureRepository(ApplicationDbContext db) : base(db)
        {

        }

        public List<ExpenditureModel> GetByParameters(ExpenditureGetListSearchParameters search)
        {
            var expenditures = GetList()
                .Where(x =>
                    (!search.AccountId.HasValue || (search.AccountId.HasValue && x.AccountId == search.AccountId.Value))
                    && (!search.StartDate.HasValue || (search.StartDate.HasValue && x.DateExpenditure >= search.StartDate))
                    && (!search.EndDate.HasValue || (search.EndDate.HasValue && x.DateExpenditure < search.EndDate))
                    && (!search.ShowOnDashboard.HasValue || (x.TypeExpenditure.ShowOnDashboard == search.ShowOnDashboard))
                    && (!search.ExpenditureTypeId.HasValue || (search.ExpenditureTypeId.HasValue && x.TypeExpenditureId == search.ExpenditureTypeId.Value)))
                .Include(u => u.Account.Currency)
                .Include(u => u.TypeExpenditure)
                .Include(u => u.PaymentMethod).ToList();

            return expenditures;
        }

        public int CountExpenditures()
        {
            return GetList().Count();
        }

        public decimal GetExpenditureCost(int id)
        {
            var entity = GetById(id);
            Refresh(entity);
            return entity.Cost;
        }
    }
}
