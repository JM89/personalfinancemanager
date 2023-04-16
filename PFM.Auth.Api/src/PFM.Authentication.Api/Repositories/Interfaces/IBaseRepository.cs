using Microsoft.EntityFrameworkCore;
using PFM.Authentication.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PFM.Authentication.Api.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> 
        where TEntity : PersistedEntity
    {
        TEntity GetById(int id, bool noTracking = false);

        DbSet<TEntity> GetList();

        IQueryable<TEntity> GetListAsNoTracking();

        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);

        void UpdateAll(List<TEntity> entities);

        bool Delete(TEntity entity);

        List<TEntity> GetList2(params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
