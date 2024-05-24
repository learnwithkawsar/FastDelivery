using Microsoft.AspNetCore.Authentication;
using FastDelivery.Framework.Infrastructure.Auth.OpenId;
using FastDelivery.Framework.Infrastructure;
using Microsoft.Extensions.Diagnostics.HealthChecks;


var builder = WebApplication.CreateBuilder(args);
bool enableSwagger = false;

var policyNames = new List<string>
{
    "catalog:read",
    "catalog:write",
    "cart:read",
    "cart:write"
};

builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
builder.Services.AddReverseProxy().LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services
              .AddHealthChecks()
              .AddCheck("self", () => HealthCheckResult.Healthy());
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapReverseProxy(config =>
{
    config.Use(async (context, next) =>
    {
        string? token = await context.GetTokenAsync("access_token");
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await next().ConfigureAwait(false);
    });
});
//app.MapReverseProxy();
app.Run();