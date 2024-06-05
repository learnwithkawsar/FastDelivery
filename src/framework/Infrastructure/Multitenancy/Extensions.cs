using Finbuckle.MultiTenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System.Reflection;

namespace FastDelivery.Framework.Infrastructure.Multitenancy;
public static class Extensions
{
    public static IServiceCollection AddMultitenancy(this IServiceCollection services, Assembly? applicationAssembly = null)
    {
        return services
            //.AddDbContext<TenantDbContext>((p, m) =>
            //{
            //    // TODO: We should probably add specific dbprovider/connectionstring setting for the tenantDb with a fallback to the main databasesettings
            //    // var databaseSettings = p.GetRequiredService<IOptions<DatabaseSettings>>().Value;
            //    string? connectionString = "Host=postgres;Port=5430;Database=authDb;Username=postgres;Password=1234;Include Error Detail=true"; // services.GetConnectionString("DefaultConnection");
            //                                                                                                                                    //m.UseDatabase(databaseSettings.DBProvider, connectionString);
            //    Console.WriteLine(applicationAssembly.FullName);
            //    m.UseNpgsql(connectionString, e =>
            //                      e.MigrationsAssembly(applicationAssembly.FullName));
            //})
            .AddMultiTenant<AppTenantInfo>()
                //.WithDelegateStrategy(context =>
                //{
                //    if (context is not HttpContext httpContext)
                //    {
                //        return Task.FromResult((string?)null);
                //    }

                //    var path = httpContext.Request.Path;

                //    // Bypass tenant resolution for /hc endpoint
                //    if (path.StartsWithSegments("/hc"))
                //    {
                //        return null; // No tenant resolution for health check endpoint
                //    }

                //    // Your existing tenant resolution logic (e.g., from header, query, etc.)
                //    var tenantId = httpContext.Request.Headers["X-Tenant-ID"].FirstOrDefault();
                //    return Task.FromResult(context);
                //})
                .WithClaimStrategy("tenant")
                .WithHeaderStrategy("tenant")
                .WithQueryStringStrategy("tenant")
                .WithConfigurationStore()

                // .WithPerTenantAuthentication()
                .Services;
        // .AddScoped<ITenantService, TenantService>();
    }

    public static IApplicationBuilder UseMultiTenancy(this IApplicationBuilder app) =>
       app.UseMultiTenant();

    private static MultiTenantBuilder<AppTenantInfo> WithQueryStringStrategy(this MultiTenantBuilder<AppTenantInfo> builder, string queryStringKey) =>
      builder.WithDelegateStrategy(context =>
      {
          if (context is not HttpContext httpContext)
          {
              return Task.FromResult((string?)null);
          }

          httpContext.Request.Query.TryGetValue(queryStringKey, out StringValues tenantIdParam);

          return Task.FromResult((string?)tenantIdParam.ToString());
      });
}
