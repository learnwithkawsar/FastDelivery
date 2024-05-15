using FastDelivery.Service.Tracking.Application;
using Microsoft.AspNetCore.Builder;
using FastDelivery.Framework.Infrastructure;
using FastDelivery.Framework.Infrastructure.Auth.OpenId;
namespace FastDelivery.Service.Tracking.Infrastructure;

public static class Extensions
{
    public static void AddTrackingInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(TrackingApplication).Assembly;
        var policyNames = new List<string> { "catalog:read", "catalog:write" };
        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
        builder.AddInfrastructure(applicationAssembly);
        //builder.Services.AddMongoDbContext<MongoDbContext>(builder.Configuration);
        //builder.Services.AddTransient<IParcelRepository, ParcelRepository>();
    }
    public static void UseTrackingInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
