using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class BankBranchRepository : BaseRepository<BankBranch>, IBankBranchRepository
    {
        public BankBranchRepository(PFMContext db) : base(db)
        {

        }
    }
}
