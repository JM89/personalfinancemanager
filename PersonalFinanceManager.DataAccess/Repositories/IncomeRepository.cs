using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class IncomeRepository : BaseRepository<IncomeModel>, IIncomeRepository
    {
        public IncomeRepository(ApplicationDbContext db) : base(db)
        {

        }

        public int CountIncomes()
        {
            return GetList().Count();
        }

        public decimal GetIncomeCost(int id)
        {
            var entity = GetById(id);
            Refresh(entity);
            return entity.Cost;
        }
    }
}
