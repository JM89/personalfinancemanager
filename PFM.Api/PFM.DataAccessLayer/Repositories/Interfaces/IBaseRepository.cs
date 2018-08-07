﻿using Microsoft.EntityFrameworkCore;
using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PFM.DataAccessLayer.Repositories.Interfaces
{
    public interface IBaseRepository<TEntity> 
        where TEntity : PersistedEntity
    {
        TEntity GetById(int id, bool noTracking = false);

        DbSet<TEntity> GetList();

        IQueryable<TEntity> GetListAsNoTracking();

        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);
        
        bool Delete(TEntity entity);

        List<TEntity> GetList2(params Expression<Func<TEntity, object>>[] includeProperties);

        TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties);

        void Refresh<T>(T entity);
    }
}
