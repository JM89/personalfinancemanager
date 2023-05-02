using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class ExpenseTypeRepository : BaseRepository<ExpenseType>, IExpenseTypeRepository
    {
        public ExpenseTypeRepository(PFMContext db) : base(db)
        {

        }
    }
}
