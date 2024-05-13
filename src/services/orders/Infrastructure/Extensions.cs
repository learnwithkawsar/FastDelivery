using FastDelivery.Service.Order.Application;
using Microsoft.AspNetCore.Builder;
using FastDelivery.Framework.Infrastructure.Auth.OpenId;
using FastDelivery.Framework.Infrastructure;
using FastDelivery.Framework.Persistence.Mongo;
using FastDelivery.Service.Order.Application.Parcels;
using FastDelivery.Service.Order.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FastDelivery.Service.Order.Infrastructure;

public static class Extensions
{
    public static void AddOrderInfrastructure(this WebApplicationBuilder builder)
    {
        var applicationAssembly = typeof(OrderApplication).Assembly;
        var policyNames = new List<string> { "catalog:read", "catalog:write" };
        builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);       
        builder.AddInfrastructure(applicationAssembly);     
        builder.Services.AddMongoDbContext<MongoDbContext>(builder.Configuration);
        builder.Services.AddTransient<IParcelRepository, ParcelRepository>();
    }
    public static void UseOrderInfrastructure(this WebApplication app)
    {
        app.UseInfrastructure(app.Environment);
    }
}
