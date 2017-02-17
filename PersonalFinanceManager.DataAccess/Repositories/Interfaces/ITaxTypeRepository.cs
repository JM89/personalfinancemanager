using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories.Interfaces
{
    public interface ITaxTypeRepository
    {
        DbSet<TaxTypeModel> GetList();
    }
}
