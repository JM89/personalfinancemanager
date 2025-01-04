using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Implementations;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> 
        where TEntity : PersistedEntity 
{
    private const string OperationTag = "db.operation.name";
    private const string QuerySummary = "db.query.summary";
    
    private const string SelectOperation = "SELECT";
    private const string DeleteOperation = "DELETE";
    private const string CreateOperation = "CREATE";
    private const string UpdateOperation = "UPDATE";
    
    public BaseRepository()
    {
    }

    public Task<List<TEntity>> GetList()
    {
        EnrichActivity(SelectOperation);
        throw new NotImplementedException();
    }

    public Task<IQueryable<TEntity>> GetListAsNoTracking()
    {
        EnrichActivity(SelectOperation);
        throw new NotImplementedException();
    }

    public Task<TEntity> GetById(int id, bool noTracking = false)
    {
        EnrichActivity(SelectOperation);
        throw new NotImplementedException();
    }

    public Task<TEntity> Create(TEntity entity)
    {
        EnrichActivity(CreateOperation);
        throw new NotImplementedException();
    }

    public Task<TEntity> Update(TEntity entity)
    {
        EnrichActivity(UpdateOperation);
        throw new NotImplementedException();
    }
    
    public Task<bool> Delete(TEntity entity)
    {
        EnrichActivity(DeleteOperation);
        throw new NotImplementedException();
    }

    public Task<TEntity> GetById(int id, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        EnrichActivity(SelectOperation);
        throw new NotImplementedException();
    }
    
    private void EnrichActivity(string operation)
    {
        Activity.Current?.SetBaggage(OperationTag, operation);
        Activity.Current?.SetBaggage(QuerySummary, $"{operation}/{typeof(TEntity).Name}");
        
        // activity.SetTag("db.name", activity.DisplayName);
        // activity.SetTag("db.query.summary", querySummary);
        // activity.SetTag("db.operation", operation);
        // activity.DisplayName = $"Run {querySummary}";
    }
}