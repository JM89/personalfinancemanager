using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using DataAccessLayer.Configurations;
using MySqlConnector;

namespace DataAccessLayer.Repositories.Implementations;

public abstract class BaseRepository<TEntity>(DatabaseOptions dbOptions, Serilog.ILogger logger) : IBaseRepository<TEntity>
    where TEntity : PersistedEntity
{
    public abstract string TableName {get; }
    
    private const string QuerySummaryTag = "db.query.summary";
    
    private const string SelectOperation = "SELECT";
    private const string DeleteOperation = "DELETE";
    private const string CreateOperation = "CREATE";
    private const string UpdateOperation = "UPDATE";

    private const string SelectSql = "SELECT * FROM {0} WHERE UserId = @userId";

    public async Task<IEnumerable<TEntity>> GetList(string userId)
    {
        EnrichActivity(SelectOperation);
        
        try
        {
            await using var connection = new MySqlConnection(dbOptions.ConnectionString);
            
            if (connection == null)
                throw new NullReferenceException("connection is null");
            
            return await connection.QueryAsync<TEntity>(string.Format(SelectSql, TableName), new
            {
                userId
            });
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Unhandled exception while listing {EntityType}",typeof(TEntity).Name);
            throw;
        }
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
        // MySql Connector Activity.
        var activity = Activity.Current;
        
        // Enrich with custom tags
        activity?.SetTag(QuerySummaryTag, $"{operation}/{typeof(TEntity).Name}");
    }
}