using PFM.DataAccessLayer.Entities;
using PFM.DataAccessLayer.SearchParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Repositories.Interfaces
{
    public interface IExpenseRepository : IBaseRepository<Expense>
    {
        List<Expense> GetByParameters(ExpenseGetListSearchParameters search);
    }
}
