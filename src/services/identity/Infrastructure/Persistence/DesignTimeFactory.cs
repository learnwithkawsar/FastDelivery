using FastDelivery.Framework.Infrastructure.Multitenancy;
using Microsoft.EntityFrameworkCore.Design;

namespace FastDelivery.Service.Identity.Infrastructure.Persistence;
public class SharedDesignTimeFactory : IDesignTimeDbContextFactory<AppIndentityDbContext>
{
    public AppIndentityDbContext CreateDbContext(string[] args)
    {
        var tenantInfo = new AppTenantInfo { ConnectionString = "Host=postgres;Port=5430;Database=authDb;Username=postgres;Password=1234;Include Error Detail=true" };
        return new AppIndentityDbContext(tenantInfo);
    }
}
