using FastDelivery.Framework.Infrastructure.Behaviors;
using FastDelivery.Framework.Infrastructure.Logging.Serilog;
using FastDelivery.Framework.Infrastructure.Mapping.Mapster;
using FastDelivery.Framework.Infrastructure.Middlewares;
using FastDelivery.Framework.Infrastructure.Options;
using FastDelivery.Framework.Infrastructure.Services;
using FastDelivery.Framework.Infrastructure.Swagger;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Infrastructure
{
    public static class Extensions
    {
        public const string AllowAllOrigins = "AllowAll";
        public static void AddInfrastructure(this WebApplicationBuilder builder, Assembly? applicationAssembly = null, bool enableSwagger = true)
        {
            var config = builder.Configuration;
            var appOptions = builder.Services.BindValidateReturn<AppOptions>(config);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: AllowAllOrigins,
                                  builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
            builder.Services.AddExceptionMiddleware();
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.ConfigureSerilog(appOptions.Name);
            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            if (applicationAssembly != null)
            {
                builder.Services.AddMapsterExtension(applicationAssembly);
                builder.Services.AddBehaviors();
                builder.Services.AddValidatorsFromAssembly(applicationAssembly);
                builder.Services.AddMediatR(o => o.RegisterServicesFromAssembly(applicationAssembly));
            }

            if (enableSwagger) builder.Services.AddSwaggerExtension(config);
          //  builder.Services.AddCachingService(config);
            builder.Services.AddInternalServices();
        }
        public static void UseInfrastructure(this WebApplication app, IWebHostEnvironment env, bool enableSwagger = true)
        {
            //Preserve Order
            app.UseCors(AllowAllOrigins);
            app.UseExceptionMiddleware();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            if (enableSwagger) app.UseSwaggerExtension(env);
        }
    }
}
