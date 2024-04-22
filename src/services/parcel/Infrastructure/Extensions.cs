using FastDelivery.Service.Parcel.Application;
using Microsoft.AspNetCore.Builder;
using FastDelivery.Framework.Infrastructure.Auth.OpenId;
using FastDelivery.Framework.Infrastructure;

namespace FastDelivery.Service.Parcel.Infrastructure
{
    public static class Extensions
    {
        public static void AddParcelInfrastructure(this WebApplicationBuilder builder)
        {
            var applicationAssembly = typeof(ParcelApplication).Assembly;
            var policyNames = new List<string> { "catalog:read", "catalog:write" };
            builder.Services.AddOpenIdAuth(builder.Configuration, policyNames);
            //builder.Services.AddMassTransit(config =>
            //{
            //    config.AddConsumers(applicationAssembly);
            //    config.UsingRabbitMq((ctx, cfg) =>
            //    {
            //        cfg.Host(builder.Configuration["RabbitMqOptions:Host"]);
            //        cfg.ConfigureEndpoints(ctx, new KebabCaseEndpointNameFormatter("catalog", false));
            //    });
            //});
            builder.AddInfrastructure(applicationAssembly);
          //  builder.Services.AddTransient<IEventPublisher, EventPublisher>();
           // builder.Services.AddMongoDbContext<MongoDbContext>(builder.Configuration);
          //  builder.Services.AddTransient<IProductRepository, ProductRepository>();
        }
        public static void UseParcelInfrastructure(this WebApplication app)
        {
            app.UseInfrastructure(app.Environment);
        }
    }
}
