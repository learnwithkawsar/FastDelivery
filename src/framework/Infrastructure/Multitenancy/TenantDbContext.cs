using Finbuckle.MultiTenant.EntityFrameworkCore.Stores.EFCoreStore;
using Finbuckle.MultiTenant.Stores;
using Microsoft.EntityFrameworkCore;

namespace FastDelivery.Framework.Infrastructure.Multitenancy;
public class TenantDbContext : EFCoreStoreDbContext<AppTenantInfo>
{
    public TenantDbContext(DbContextOptions<TenantDbContext> options)
        : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppTenantInfo>().ToTable("Tenants", "MultiTenancy");
    }
}