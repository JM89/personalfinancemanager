using PFM.DataAccessLayer.Repositories.Interfaces;
using PFM.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace PFM.DataAccessLayer.Repositories.Implementations
{
    public class BaseRepository<TEntity>(PFMContext db) : IBaseRepository<TEntity>
        where TEntity : PersistedEntity
    {
        private const string OperationTag = "db.operation.name";
        private const string QuerySummary = "db.query.summary";
        
        private const string SelectOperation = "SELECT";
        private const string DeleteOperation = "DELETE";
        private const string CreateOperation = "CREATE";
        private const string UpdateOperation = "UPDATE";

        public DbSet<TEntity> GetList()
        {
            EnrichActivity(SelectOperation);
            return db.Set<TEntity>();
        }

        public IQueryable<TEntity> GetListAsNoTracking()
        {
            EnrichActivity(SelectOperation);
            return db.Set<TEntity>().AsNoTracking();
        }

        public TEntity GetById(int id, bool noTracking = false)
        {            
            EnrichActivity(SelectOperation);
            if (noTracking)
            {
                return GetList().AsNoTracking().Single(x => x.Id == id);
            }
            return GetList().Find(id);
        }

        public TEntity Create(TEntity entity)
        {
            EnrichActivity(CreateOperation);
            GetList().Add(entity);
            db.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            EnrichActivity(UpdateOperation);
            db.Entry(entity).State = EntityState.Modified;
            db.SaveChanges();
            return entity;
        }

        public void UpdateAll (List<TEntity> entities)
        {
            EnrichActivity(UpdateOperation);
            foreach(var entity in entities)
            {
                db.Entry(entity).State = EntityState.Modified;
            }
            db.SaveChanges();
        }

        public bool Delete(TEntity entity)
        {
            EnrichActivity(DeleteOperation);
            GetList().Remove(entity);
            db.SaveChanges();
            return true;
        }
        
        public List<TEntity> GetList2(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            EnrichActivity(SelectOperation);
            
            var result = db.Set<TEntity>();

            IQueryable<TEntity> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query?.ToList() ?? result.ToList();
        }

        public TEntity GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            EnrichActivity(SelectOperation);
            
            var result = db.Set<TEntity>();

            IQueryable<TEntity> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property) : query.Include(property);
            }

            return query?.Single(x => x.Id == id) ?? result.Single(x => x.Id == id); 
        }

        public void Refresh<T>(T entity)
        {
            //var ctx = ((IObjectContextAdapter)_db).ObjectContext;
            //ctx.Refresh(RefreshMode.StoreWins, entity);
            throw new NotImplementedException("Regression from migrations");
        }
        
        private void EnrichActivity(string operation)
        {
            Activity.Current?.SetBaggage(OperationTag, operation);
            Activity.Current?.SetBaggage(QuerySummary, $"{operation}/{typeof(TEntity).Name}");
        }
    }
}
