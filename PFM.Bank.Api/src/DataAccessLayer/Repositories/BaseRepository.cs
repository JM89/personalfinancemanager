using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories
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

        void Refresh<T>(T entity);
    }
    
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> 
        where TEntity : PersistedEntity 
    {
        private readonly PFMContext _db;
        
        private const string OperationTag = "db.operation.name";
        private const string QuerySummary = "db.query.summary";
        
        private const string SelectOperation = "SELECT";
        private const string DeleteOperation = "DELETE";
        private const string CreateOperation = "CREATE";
        private const string UpdateOperation = "UPDATE";
        
        public BaseRepository(PFMContext db)
        {
            this._db = db;
        }

        public DbSet<TEntity> GetList()
        {
            EnrichActivity(SelectOperation);
            return _db.Set<TEntity>();
        }

        public IQueryable<TEntity> GetListAsNoTracking()
        {
            EnrichActivity(SelectOperation);
            return _db.Set<TEntity>().AsNoTracking();
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
            _db.SaveChanges();
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            EnrichActivity(UpdateOperation);
            _db.Entry(entity).State = EntityState.Modified;
            _db.SaveChanges();
            return entity;
        }

        public void UpdateAll (List<TEntity> entities)
        {
            EnrichActivity(UpdateOperation);
            
            foreach(var entity in entities)
            {
                _db.Entry(entity).State = EntityState.Modified;
            }
            _db.SaveChanges();
        }

        public bool Delete(TEntity entity)
        {
            EnrichActivity(DeleteOperation);
            
            GetList().Remove(entity);
            _db.SaveChanges();
            return true;
        }

        public List<TEntity> GetList2(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            EnrichActivity(SelectOperation);
            
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
            EnrichActivity(SelectOperation);
            
            var result = _db.Set<TEntity>().AsNoTracking();

            IQueryable<TEntity> query = null;
            foreach (var property in includeProperties)
            {
                query = query == null ? result.Include(property).AsNoTracking() : query.Include(property).AsNoTracking();
            }

            return query?.Single(x => x.Id == id) ?? result.Single(x => x.Id == id); 
        }
        
        private void EnrichActivity(string operation)
        {
            Activity.Current?.SetBaggage(OperationTag, operation);
            Activity.Current?.SetBaggage(QuerySummary, $"{operation}/{typeof(TEntity).Name}");
        }

        public void Refresh<T>(T entity)
        {
            //var ctx = ((IObjectContextAdapter)_db).ObjectContext;
            //ctx.Refresh(RefreshMode.StoreWins, entity);
            throw new NotImplementedException("Regression from migrations");
        }
    }
}
