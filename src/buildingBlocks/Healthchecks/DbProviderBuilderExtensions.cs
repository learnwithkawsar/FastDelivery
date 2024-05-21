using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;


public static class DbProviderBuilderExtensions
{
    public static IHealthChecksBuilder AddNpgSQL(this IHealthChecksBuilder builder, IConfiguration configuration)
    {

        return builder.AddNpgSql(configuration.GetConnectionString("DefaultConnection"));
    }
    public static IHealthChecksBuilder AddMongoDb(this IHealthChecksBuilder builder, IConfiguration configuration)
    {

        return builder.AddMongoDb(
          mongodbConnectionString: configuration.GetSection("MongoOptions:ConnectionString").Value.ToString());
    }

}
