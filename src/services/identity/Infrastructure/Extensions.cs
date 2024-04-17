using FastDelivery.Framework.Infrastructure;
using FastDelivery.Framework.Infrastructure.Auth.OpenIddict;
using FastDelivery.Service.Identity.Application;
using FastDelivery.Service.Identity.Domain.Users;
using FastDelivery.Service.Identity.Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace FastDelivery.Service.Identity.Infrastructure;
public static class Extensions
{
    internal static bool enableSwagger = true;
    public static void AddIdentityInfrastructure(this WebApplicationBuilder builder)
    {
        var coreAssembly = typeof(IdentityCore).Assembly;
        var dbContextAssembly = typeof(AppIndentityDbContext).Assembly;

        builder.Services.AddIdentityExtensions();
        builder.AddInfrastructure(applicationAssembly: coreAssembly, enableSwagger: enableSwagger);
        builder.ConfigureAuthServer<AppIndentityDbContext>(dbContextAssembly);
        builder.Services.AddHostedService<SeedClientsAndScopes>();
    }
    public static void UseIdentityInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment, enableSwagger);
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
