using FastDelivery.Framework.Infrastructure.Options;

namespace FastDelivery.Framework.Persistence.Mongo;
public class MongoOptions : IOptionsRoot
{
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
}

