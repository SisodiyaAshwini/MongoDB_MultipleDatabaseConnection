using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace DataAccess
{
    public class MongoDbConnectionFactory
    {
        public interface IMongoDBConnectionFactory<TEntity> where TEntity : class
        {
            IMongoCollection<TEntity> GetCollection();
        }

        public class MongoDBConnectionFactory<TEntity> : IMongoDBConnectionFactory<TEntity> where TEntity : class
        {
            private readonly IOptionsSnapshot<MongoDbConnection> _config;
            public MongoDBConnectionFactory(IOptionsSnapshot<MongoDbConnection> config)
            {
                _config = config;
            }

            public IMongoCollection<TEntity> GetCollection()
            {
                var database = GetConnection(ConnectionDB.BaseConnection.ToString());
                string collectionName = GetCustomAttribute();
                if (collectionName == null) collectionName = typeof(TEntity).Name;
                if(!CollectionExists(database, collectionName))
                {
                    database = GetConnection(ConnectionDB.BackgroundJobs.ToString());
                    if (CollectionExists(database, collectionName))
                        return database.GetCollection<TEntity>(collectionName);
                }
                return database.GetCollection<TEntity>(collectionName);
            }

            private IMongoDatabase GetConnection(string connectionDb)
            {
                var connection = _config.Get(connectionDb);
                var client = new MongoClient(connection.ConnectionString);
                return client.GetDatabase(connection.DatabaseName);                
            }

            private bool CollectionExists(IMongoDatabase database, string collectionName)
            {
                var filter = new BsonDocument("name", collectionName);
                var options = new ListCollectionNamesOptions { Filter = filter };

                return database.ListCollectionNames(options).Any();
            }

            private string GetCustomAttribute()
            {
                if (typeof(TEntity).GetCustomAttributes(typeof(EntityAttribute), true).FirstOrDefault() is EntityAttribute attribute)
                {
                    return attribute.Name;
                }
                return null;
            }
        }
    }
}
