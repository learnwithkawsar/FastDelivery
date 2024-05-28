using FastDelivery.Framework.Infrastructure;
using FastDelivery.Framework.Infrastructure.Auth.OpenId;
using FastDelivery.Framework.Persistence.Mongo;
using FastDelivery.Service.Tracking.Application;
using FastDelivery.Service.Tracking.Application.IntegrationEvents.EventHandling;
using FastDelivery.Service.Tracking.Application.Trackings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
namespace FastDelivery.Service.Tracking.Infrastructure;

public static class Extensions
{
    public static void AddTrackingInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(TrackingApplication).Assembly;
        var policyNames = new List<string> { "catalog:read", "catalog:write" };
        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(applicationAssembly, false);

        builder.Services
                .AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy())
                .AddDapr()
                .AddMongoDb(builder.Configuration);

        builder.Services.AddMongoDbContext<MongoDbContext>(builder.Configuration);
        builder.Services.AddTransient<ITrackingRepository, TrackingRepository>();
        builder.Services.AddScoped<OrderAddToTrackingIntegrationEventHandler>();
    }
    public static void UseTrackingInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);

    }
}
