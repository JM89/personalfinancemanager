using Microsoft.EntityFrameworkCore;
using PFM.Authentication.Api.Entities;
using PFM.Authentication.Api.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PFM.Authentication.Api.Repositories.Implementations
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> 
        where TEntity : PersistedEntity 
    {
        protected readonly PFMContext _db;

        public BaseRepository(PFMContext db)
        {
            this._db = db;
        }

        public DbSet<TEntity> GetList()
        {
            return _db.Set<TEntity>();
        }

        public IQueryable<TEntity> GetListAsNoTracking()
        {
            return _db.Set<TEntity>().AsNoTracking();
        }

        public TEntity GetById(int id, bool noTracking = false)
        {
            if (noTracking)
            {
                return GetList().AsNoTracking().Single(x => x.Id == id);
            }
            return GetList().Find(id);
        }

        public TEntity Create(TEntity entity)
        {
            GetList().Add(entity);
            _db.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
            return entity;
        }

        public void UpdateAll (List<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                _db.Entry(entity).State = EntityState.Modified;
            }
            _db.SaveChanges();
        }

        public bool Delete(TEntity entity)
        {
            GetList().Remove(entity);
            _db.SaveChanges();
            return true;
        }
        
        public List<TEntity> GetList2(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var result = _db.Set<TEntity>();

            IQueryable<TEntity> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query?.ToList() ?? result.ToList();
        }

        public TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var result = _db.Set<TEntity>();

            IQueryable<TEntity> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query?.Single(x => x.Id == id) ?? result.Single(x => x.Id == id); 
        }
    }
}
