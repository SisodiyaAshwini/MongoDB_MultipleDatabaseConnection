using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static DataAccess.MongoDbConnectionFactory;

namespace DataAccess
{
    public class MongoDbDataRepository<TEntity> : IDataRepository<TEntity>
        where TEntity : class
    {
        private readonly IMongoCollection<TEntity> _collection;
        private readonly IMongoDBConnectionFactory<TEntity> _connectionFactory;
        public MongoDbDataRepository(IMongoDBConnectionFactory<TEntity> connectionFactory)
        {
            _connectionFactory = connectionFactory;
            _collection = _connectionFactory.GetCollection();
        }
        
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            var s = _collection.AsQueryable();
            return await _collection.AsQueryable()?.ToListAsync();
        }

        public async Task<TEntity> GetOneAsync(Expression<Func<TEntity, bool>> filter = null)
        {
            if (filter != null)
                return await _collection.Find(filter).SingleOrDefaultAsync();
            else
                return await _collection.AsQueryable().FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateOneAsync(Expression<Func<TEntity, bool>> filter, TEntity value)
        {
            var result = await _collection.ReplaceOneAsync(filter, value);
            return result.ModifiedCount == 1;
        }

        public async Task<long> UpdateOnePushAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, IEnumerable<TField>>> field, TField value)
        {
            var result = await _collection.UpdateOneAsync(filter, Builders<TEntity>.Update.Push(field, value));
            return result.ModifiedCount;
        }
        public async Task<long> UpdateOneSetAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> field, TField value)
        {
            var result = await _collection.UpdateOneAsync(filter, Builders<TEntity>.Update.Set(field, value));
            return result.ModifiedCount;
        }


        public async Task<long> UpdateOneAsync<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, IEnumerable<TField>>> fieldArray, Expression<Func<TField, bool>> field)
        {
            var update = Builders<TEntity>.Update.PullFilter(fieldArray, field);
            var result = await _collection.UpdateOneAsync(filter, update);
            return result.ModifiedCount;
        }

        public async Task<TEntity> InsertOneAsync(TEntity entity)
        {
            await _collection.InsertOneAsync(entity);
            return entity;
        }
    }
}
