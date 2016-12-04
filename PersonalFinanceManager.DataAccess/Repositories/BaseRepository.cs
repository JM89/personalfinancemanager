using PersonalFinanceManager.DataAccess.Repositories.Interfaces;
using PersonalFinanceManager.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalFinanceManager.DataAccess.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> 
        where TEntity : PersistedEntity 
    {
        private readonly ApplicationDbContext _db;

        public BaseRepository(ApplicationDbContext db)
        {
            this._db = db;
        }

        public DbSet<TEntity> GetList()
        {
            return _db.Set(typeof(TEntity)).Cast<TEntity>();
        }

        public TEntity GetById(int id)
        {
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

        public bool Delete(TEntity entity)
        {
            GetList().Remove(entity);
            _db.SaveChanges();
            return true;
        }
    }
}
