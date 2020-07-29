using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccess
{
    public interface IDataRepository<TEntity>
        where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> InsertOneAsync(TEntity entity);
        Task<bool> UpdateOneAsync(Expression<Func<TEntity, bool>> filter, TEntity value);
        Task<long> UpdateOnePushAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, IEnumerable<TField>>> field, TField value);
        Task<long> UpdateOneSetAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> field, TField value);
        Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null);
        Task<long> UpdateOneAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, IEnumerable<TField>>> fieldArray, Expression<Func<TField, bool>> field);
    }
}
