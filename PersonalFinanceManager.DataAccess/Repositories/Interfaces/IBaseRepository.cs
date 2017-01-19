using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> 
        where TEntity : PersistedEntity
    {
        TEntity GetById(int id, bool noTracking = false);

        DbSet<TEntity> GetList();

        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);
        
        bool Delete(TEntity entity);
    }
}
