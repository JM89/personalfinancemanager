using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> 
        where TEntity : PersistedEntity
    {
        Task<IEnumerable<TEntity>> GetList(string userId);

        Task<TEntity> Create(TEntity entity);

        Task<TEntity> Update(TEntity entity);
        
        Task<bool> Delete(TEntity entity);
        
        Task<TEntity> GetById(Guid id);
    }
}
