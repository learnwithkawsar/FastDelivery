using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Validation.AspNetCore;
using System.Reflection;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace FastDelivery.Framework.Infrastructure.Auth.OpenIddict;
public static class Extensions
{
    public static void ConfigureAuthServer<T>(this WebApplicationBuilder builder, Assembly dbContextAssembly, string connectionName = "DefaultConnection") where T : DbContext
    {
        builder.Services.AddOpenIddict()
        .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<T>())
        .AddServer(options =>
        {
            options.SetAuthorizationEndpointUris("/connect/authorize")
                   .SetIntrospectionEndpointUris("/connect/introspect")
                   .SetUserinfoEndpointUris("connect/userinfo")
                   .SetTokenEndpointUris("/connect/token");
            options.AllowClientCredentialsFlow();
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

        string? connectionString = builder.Configuration.GetConnectionString(connectionName);
        if (!builder.Environment.IsDevelopment() && connectionString == null)
            throw new ArgumentNullException(nameof(connectionString));

        builder.Services.AddDbContext<T>(options =>
        {
            if (builder.Environment.IsDevelopment())
            {
                // options.UseInMemoryDatabase("authDb");
                options.UseNpgsql(connectionString, m => m.MigrationsAssembly(dbContextAssembly.FullName));

            }
            else
            {
                options.UseNpgsql(connectionString, m => m.MigrationsAssembly(dbContextAssembly.FullName));
            }
            options.UseOpenIddict();
        });
    }
}
