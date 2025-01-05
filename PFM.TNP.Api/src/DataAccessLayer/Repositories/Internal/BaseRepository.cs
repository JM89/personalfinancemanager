using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DataAccessLayer.Configurations;
using MySqlConnector;

namespace DataAccessLayer.Repositories.Internal;

public interface IBaseRepository<TEntity> 
    where TEntity : PersistedEntity
{
    Task<IEnumerable<TEntity>> GetList(string userId);

    Task<bool> Create(TEntity entity);

    Task<TEntity> Update(TEntity entity);
        
    Task<bool> Delete(Guid id);
        
    Task<TEntity> GetById(Guid id);
}

public abstract class BaseRepository<TEntity>(DatabaseOptions dbOptions, Serilog.ILogger logger) : IBaseRepository<TEntity>
    where TEntity : PersistedEntity
{
    protected abstract string TableName {get; }
    
    private const string QuerySummaryTag = "db.query.summary";
    
    private const string SelectOperation = "SELECT";
    private const string DeleteOperation = "DELETE";
    private const string CreateOperation = "CREATE";
    private const string UpdateOperation = "UPDATE";

    private const string SelectSql = "SELECT * FROM {0} WHERE UserId = @UserId";
    private const string SelectByIdSql = "SELECT * FROM {0} WHERE Id = @Id";
    
    private const string InsertSql = "INSERT INTO {0} ({1}) VALUES ({2})";
    private const string DeleteSql = "DELETE FROM {0} WHERE Id = @Id";
    
    public async Task<IEnumerable<TEntity>> GetList(string userId)
    {
        var sql = string.Format(SelectSql, TableName);
        return await ApplyOperation(SelectOperation, async (connection, input) => await connection.QueryAsync<TEntity>(sql, input), new {UserId = userId}); 
    }
    
    public async Task<bool> Create(TEntity entity)
    {
        var props = typeof(TEntity).GetProperties();
        var insertProps = string.Join(",", props.Select(x => x.Name));
        var insertValues = string.Join(",", props.Select(x => $"@{x.Name}"));
        var sql = string.Format(InsertSql, TableName, insertProps, insertValues);
        
        return await ApplyOperation(CreateOperation, async (connection, input) => await connection.ExecuteAsync(sql, input) == 1, entity); 
    }
    
    public Task<TEntity> Update(TEntity entity)
    {
        EnrichActivity(UpdateOperation);
        throw new NotImplementedException();
    }
    
    public async Task<bool> Delete(Guid id)
    {
        var sql = string.Format(DeleteSql, TableName);
        return await ApplyOperation(DeleteOperation, async (connection, input) => await connection.ExecuteAsync(sql, input) == 1, new {Id = id}); 
    }

    public async Task<TEntity> GetById(Guid id)
    {
        var sql = string.Format(SelectByIdSql, TableName);
        return await ApplyOperation(SelectOperation, async (connection, input) => await connection.QuerySingleOrDefaultAsync<TEntity>(sql, input), new {Id = id}); 
    }
    
    private async Task<TOutput> ApplyOperation<TInput,TOutput>(string operation, Func<MySqlConnection, TInput, Task<TOutput>> func, TInput input)
    {
        EnrichActivity(operation);
        
        try
        {
            await using var connection = new MySqlConnection(dbOptions.ConnectionString);
            
            if (connection == null)
                throw new NullReferenceException("connection is null");
            
            return await func(connection, input);
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Unhandled exception while {operation} {EntityType}",operation, input.GetType().Name);
            throw;
        }
    }
    
    private void EnrichActivity(string operation)
    {
        // MySql Connector Activity.
        var activity = Activity.Current;
        
        // Enrich with custom tags
        activity?.SetTag(QuerySummaryTag, $"{operation}/{typeof(TEntity).Name}");
    }
}