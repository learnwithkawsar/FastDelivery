using FastDelivery.Framework.Infrastructure.Multitenancy;
using FastDelivery.Service.Identity.Domain.Users;
using Finbuckle.MultiTenant.Abstractions;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FastDelivery.Service.Identity.Infrastructure.Persistence;

public class AppIndentityDbContext : MultiTenantIdentityDbContext<ApplicationUser>
{
    // private AppTenantInfo TenantInfo { get; set; }
    public AppIndentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor, DbContextOptions options) : base(multiTenantContextAccessor, options)
    {
        // TenantInfo = multiTenantContextAccessor.TenantInfo;
        // TenantInfo = multiTenantContextAccessor.MultiTenantContext?.TenantInfo as AppTenantInfo;
    }

    public AppIndentityDbContext(IMultiTenantContextAccessor multiTenantContextAccessor) : base(multiTenantContextAccessor)
    {

    }


    public AppIndentityDbContext(ITenantInfo tenantInfo) : base(tenantInfo)
    {
        // used for the design-time factory and progammatic migrations in program.cs
    }

    //public AppIndentityDbContext(ITenantInfo tenantInfo) : base(tenantInfo)
    //{

    //}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // var tenantInfo = TenantInfo as AppTenantInfo;
        optionsBuilder.UseNpgsql("Host=postgres;Port=5430;Database=authDb;Username=postgres;Password=1234;Include Error Detail=true");
        optionsBuilder.EnableSensitiveDataLogging();
        //  base.OnConfiguring(optionsBuilder);
        //    // TODO: We want this only for development probably... maybe better make it configurable in logger.json config?


        //    // If you want to see the sql queries that efcore executes:

        //    // Uncomment the next line to see them in the output window of visual studio
        //    // optionsBuilder.LogTo(m => System.Diagnostics.Debug.WriteLine(m), Microsoft.Extensions.Logging.LogLevel.Information);

        //    // Or uncomment the next line if you want to see them in the console
        //    // optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);

        //if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
        //{
        //    optionsBuilder.UseNpgsql(TenantInfo?.ConnectionString ?? throw new InvalidOperationException());
        //}
    }
}
