using FastDelivery.Framework.Infrastructure;
using FastDelivery.Framework.Infrastructure.Multitenancy;
using FastDelivery.Service.Identity.Application;
using FastDelivery.Service.Identity.Domain.Users;
using FastDelivery.Service.Identity.Infrastructure.Persistence;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation.AspNetCore;
using System.Reflection;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace FastDelivery.Service.Identity.Infrastructure;
public static class Extensions
{
    internal static bool enableSwagger = false;
    public static void AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        var coreAssembly = typeof(IdentityCore).Assembly;
        var dbContextAssembly = typeof(AppIndentityDbContext).Assembly;

        builder.Services.AddMultitenancy(dbContextAssembly);
        //  builder.Services.AddScoped<IMultiTenantContextAccessor<AppTenantInfo>, MultiTenantContextAccessor<AppTenantInfo>>();
        builder.Services.AddIdentityExtensions();
        builder.ConfigureAuthServer(dbContextAssembly);

        builder.AddInfrastructure(applicationAssembly: coreAssembly, enableSwagger: enableSwagger);


        //ApplyMigration<AppIndentityDbContext>(builder.Services.BuildServiceProvider());
        // ApplyMigration<TenantDbContext>(builder.Services.BuildServiceProvider());



        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
        builder.Services
               .AddHealthChecks()
               .AddCheck("self", () => HealthCheckResult.Healthy())
               .AddDapr()
               .AddNpgSQL(builder.Configuration);
        builder.Services.AddHostedService<SeedClientsAndScopes>();
    }
    public static async void ApplyMigration<T>(IServiceProvider serviceProvider) where T : DbContext
    {



        // Get an instance of your DbContext
        using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {

            var dbContext = serviceScope.ServiceProvider.GetRequiredService<T>();

            if (dbContext.Database.GetPendingMigrations().Any())
            {
                // Apply pending migrations
                dbContext.Database.Migrate();
            }

        }
    }


    public static void ConfigureAuthServer(this WebApplicationBuilder builder, Assembly dbContextAssembly, string connectionName = "DefaultConnection")
    {
        string? connectionString = builder.Configuration.GetConnectionString(connectionName);
        if (!builder.Environment.IsDevelopment() && connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));
        builder.Services.AddOpenIddict()
        .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<AppIndentityDbContext>())
        .AddServer(options =>
        {
            options.SetAuthorizationEndpointUris("/connect/authorize")
                   .SetIntrospectionEndpointUris("/connect/introspect")
                   .SetUserinfoEndpointUris("connect/userinfo")
                   .SetTokenEndpointUris("/connect/token");
            options.AllowClientCredentialsFlow();
            options.AllowPasswordFlow();
            options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.Roles);
            options.DisableAccessTokenEncryption();
            options.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();
            options.UseAspNetCore().EnableTokenEndpointPassthrough().DisableTransportSecurityRequirement();
        })
        .AddValidation(options =>
        {
            options.UseLocalServer();
            options.UseAspNetCore();
        });

        builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        builder.Services.AddAuthorization();


        builder.Services.AddDbContext<AppIndentityDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseOpenIddict();
        });
        //builder.Services.AddDbContext<AppIndentityDbContext>((serviceProvider, options) =>
        //{
        //    //var tenantContextAccessor = serviceProvider.GetRequiredService<IMultiTenantContextAccessor<AppTenantInfo>>();
        //    //var tenantInfo = tenantContextAccessor.MultiTenantContext?.TenantInfo;
        //    if (builder.Environment.IsDevelopment())
        //    {
        //        // options.UseInMemoryDatabase("authDb");
        //        options.UseNpgsql(connectionString, m => m.MigrationsAssembly(dbContextAssembly.FullName));

        //    }
        //    else
        //    {
        //        options.UseNpgsql(connectionString, m => m.MigrationsAssembly(dbContextAssembly.FullName));
        //    }
        //    options.UseOpenIddict();
        //});

    }
    public static async void UseIdentityInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment, enableSwagger);
        app.UseMiddleware<TenantLoggingMiddleware>();
        // Apply migrations if needed
        //var store = app.Services.GetRequiredService<IMultiTenantStore<AppTenantInfo>>();
        //foreach (var tenant in await store.GetAllAsync())
        //{
        //    await using var db = new AppIndentityDbContext(tenant);
        //    await db.Database.MigrateAsync();
        //}

    }
    internal static IServiceCollection AddIdentityExtensions(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.User.RequireUniqueEmail = true;
        })
         .AddEntityFrameworkStores<AppIndentityDbContext>()
        .AddDefaultTokenProviders();


        services.Configure<IdentityOptions>(options =>
        {
            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            options.ClaimsIdentity.UserNameClaimType = Claims.Name;
            options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
            options.ClaimsIdentity.RoleClaimType = Claims.Role;
            options.ClaimsIdentity.EmailClaimType = Claims.Email;

            // Note: to require account confirmation before login,
            // register an email sender service (IEmailSender) and
            // set options.SignIn.RequireConfirmedAccount to true.
            //
            // For more information, visit https://aka.ms/aspaccountconf.
            options.SignIn.RequireConfirmedAccount = false;
        });
        return services;


    }
}
