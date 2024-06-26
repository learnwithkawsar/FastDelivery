﻿using FastDelivery.Framework.Infrastructure.Behaviors;
using FastDelivery.Framework.Infrastructure.Common;
using FastDelivery.Framework.Infrastructure.Logging.Serilog;
using FastDelivery.Framework.Infrastructure.Mapping.Mapster;
using FastDelivery.Framework.Infrastructure.Middlewares;
using FastDelivery.Framework.Infrastructure.Multitenancy;
using FastDelivery.Framework.Infrastructure.Options;
using FastDelivery.Framework.Infrastructure.Services;
using FastDelivery.Framework.Infrastructure.Swagger;
using Finbuckle.MultiTenant;
using FluentValidation;
using Google.Api;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using System.Reflection;

namespace FastDelivery.Framework.Infrastructure;

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
        builder.Services.AddMemoryCache();
        builder.Services.AddCustomeApiVersioning();
        builder.Services.AddExceptionMiddleware();
        builder.Services.AddControllers().AddDapr();
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

        if (enableSwagger)
        {
            builder.Services.AddSingleton<ExcludedPathsService>();
            builder.Services.AddSwaggerExtension(config);
        }
        //  builder.Services.AddCachingService(config);
        builder.Services.AddInternalServices();
        builder.Services.AddDaprClient();
        //builder.Services.AddHttpLogging(logging =>
        //{
        //    logging.LoggingFields = HttpLoggingFields.All;
        //    logging.RequestHeaders.Add(HeaderNames.Accept);
        //    logging.RequestHeaders.Add(HeaderNames.ContentType);
        //    logging.RequestHeaders.Add(HeaderNames.ContentDisposition);
        //    logging.RequestHeaders.Add(HeaderNames.ContentEncoding);
        //    logging.RequestHeaders.Add(HeaderNames.ContentLength);

        //    logging.MediaTypeOptions.AddText("application/json");
        //    logging.MediaTypeOptions.AddText("multipart/form-data");

        //    logging.RequestBodyLogLimit = 4096;
        //    logging.ResponseBodyLogLimit = 4096;
        //});



    }

    public static void UseInfrastructure(this WebApplication app, IWebHostEnvironment env, bool enableSwagger = true)
    {
        //Preserve Order
        // app.UseHttpLogging();
        app.UseCloudEvents();
        app.UseCors(AllowAllOrigins);
        app.UseExceptionMiddleware();
        app.UseRouting();
        app.UseMultiTenancy();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapSubscribeHandler();
        app.MapControllers();



        //app.UseSerilogRequestLogging(opt => {
        //    opt.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        //    {
        //         string body = httpContext.Request.Body;
        //        diagnosticContext.Set("Body", body);
        //    };
        //});
        // app.MapSubscribeHandler();
        if (enableSwagger) app.UseSwaggerExtension(env);
        app.MapCustomHealthChecks("/hc", "/liveness", UIResponseWriter.WriteHealthCheckUIResponse);
    }
}
