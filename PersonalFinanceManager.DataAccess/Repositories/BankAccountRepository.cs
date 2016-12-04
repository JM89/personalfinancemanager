using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class BankAccountRepository : BaseRepository<AccountModel>, IBankAccountRepository
    {
        public BankAccountRepository(ApplicationDbContext db) : base(db)
        {

        }
    }
}
