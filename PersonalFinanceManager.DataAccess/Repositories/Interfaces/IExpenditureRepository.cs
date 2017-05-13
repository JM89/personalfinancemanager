using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalFinanceManager.Entities.SearchParameters;

namespace PersonalFinanceManager.DataAccess.Repositories.Interfaces
{
    public interface IExpenditureRepository : IBaseRepository<ExpenditureModel>
    {
        List<ExpenditureModel> GetByParameters(ExpenditureGetListSearchParameters search);

        int CountExpenditures();

        decimal GetExpenditureCost(int id);
    }
}
