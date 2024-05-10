using MongoDB.Driver;

namespace FastDelivery.Framework.Persistence.Mongo;
public interface IMongoDbContext : IDisposable
{
    IMongoCollection<T> GetCollection<T>(string? name = null);
}
