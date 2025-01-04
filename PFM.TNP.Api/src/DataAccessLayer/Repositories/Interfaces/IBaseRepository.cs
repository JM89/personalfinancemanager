using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> 
        where TEntity : PersistedEntity
    {
        Task<TEntity> GetById(int id, bool noTracking = false);

        Task<IEnumerable<TEntity>> GetList(string userId);

        Task<IQueryable<TEntity>> GetListAsNoTracking();

        Task<TEntity> Create(TEntity entity);

        Task<TEntity> Update(TEntity entity);
        
        Task<bool> Delete(TEntity entity);
        
        Task<TEntity> GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
