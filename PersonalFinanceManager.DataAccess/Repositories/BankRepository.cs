using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class BankRepository : BaseRepository<BankModel>, IBankRepository
    {
        public BankRepository(ApplicationDbContext db) : base(db)
        {

        }
    }
}
