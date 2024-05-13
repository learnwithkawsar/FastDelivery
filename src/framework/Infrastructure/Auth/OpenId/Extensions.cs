using FastDelivery.Framework.Core.Exceptions;
using FastDelivery.Framework.Infrastructure.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace FastDelivery.Framework.Infrastructure.Auth.OpenId;
public static class Extensions
{
    public static IServiceCollection AddOpenIdAuth(this IServiceCollection services, IConfiguration config, List<string> policyNames)
    {
        var authOptions = services.BindValidateReturn<OpenIdOptions>(config);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            // options.UseSecurityTokenValidators = true;
            options.Authority = authOptions.Authority;
            options.Audience = authOptions.Audience;
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateActor = false,
                ValidateIssuerSigningKey = false,
                ValidateTokenReplay = false,
                SignatureValidator = (token, _) => new JsonWebToken(token)
            };
            options.Events = new JwtBearerEvents
            {
                OnTokenValidated = async context =>
                {
                    var identity = context.Principal.Identity as ClaimsIdentity;
                    if (identity != null)
                    {
                        // List all claims for debugging purposes
                        foreach (var claim in identity.Claims)
                        {
                            //   Console.WriteLine($"Claim: {claim.Type} - {claim.Value}");
                        }
                    }

                },
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    if (!context.Response.HasStarted)
                    {
                        throw new UnauthorizedException(context.Error!, context.ErrorDescription!);
                    }

                    return Task.CompletedTask;
                },
                OnForbidden = _ => throw new ForbiddenException()
            };
        });

        if (policyNames?.Count > 0)
        {
            services.AddAuthorization(options =>
            {
                foreach (string policyName in policyNames)
                {
                    options.AddPolicy(policyName, policy => policy.Requirements.Add(new HasScopeRequirement(policyName, authOptions.Authority!)));
                }
            });
        }

        services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
        return services;
    }
}
