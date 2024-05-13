using Microsoft.AspNetCore.Builder;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastDelivery.Framework.Infrastructure.Options;
using Serilog.Exceptions;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Elasticsearch;
using System.Reflection;

namespace FastDelivery.Framework.Infrastructure.Logging.Serilog;
public static class Extensions
{
    public static void ConfigureSerilog(this WebApplicationBuilder builder, string appName)
    {
        var config = builder.Configuration;
        var serilogOptions = builder.Services.BindValidateReturn<SerilogOptions>(config);
        _ = builder.Host.UseSerilog((_, _, serilogConfig) =>
        {
            if (serilogOptions.EnableErichers) ConfigureEnrichers(serilogConfig, appName);
            ConfigureConsoleLogging(serilogConfig, serilogOptions.StructuredConsoleLogging);
            ConfigureWriteToFile(serilogConfig, serilogOptions.WriteToFile, serilogOptions.RetentionFileCount, appName);
            ConfigureWriteToElasticsearch(serilogConfig, serilogOptions);
            SetMinimumLogLevel(serilogConfig, serilogOptions.MinimumLogLevel);
            if (serilogOptions.OverideMinimumLogLevel) OverideMinimumLogLevel(serilogConfig);
        });
    }

    private static void ConfigureEnrichers(LoggerConfiguration config, string appName)
    {
        config
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", appName)
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId();
    }

    private static void ConfigureConsoleLogging(LoggerConfiguration serilogConfig, bool structuredConsoleLogging)
    {
        if (structuredConsoleLogging)
        {
            serilogConfig.WriteTo.Async(wt => wt.Console(new CompactJsonFormatter()));
        }
        else
        {
            serilogConfig.WriteTo.Async(wt => wt.Console());
        }
    }

    private static void ConfigureWriteToFile(LoggerConfiguration serilogConfig, bool writeToFile, int retainedFileCount, string appName)
    {
        if (writeToFile)
        {
            serilogConfig.WriteTo.File(
             new CompactJsonFormatter(),
             $"Logs/{appName.ToLower()}.logs.json",
             restrictedToMinimumLevel: LogEventLevel.Information,
             rollingInterval: RollingInterval.Day,
             retainedFileCountLimit: retainedFileCount);
        }
    }

    private static void ConfigureWriteToElasticsearch(LoggerConfiguration serilogConfig, SerilogOptions serilogOptions)
    {
        if (serilogOptions.WriteToElastic)
        {
            serilogConfig.WriteTo.Elasticsearch(options: new ElasticsearchSinkOptions(new Uri(serilogOptions.ElasticSearchUrl))
            {
                MinimumLogEventLevel = LogEventLevel.Information,
                AutoRegisterTemplate = true,
                IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-{serilogOptions.AppName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}"
                //{Assembly.GetExecutingAssembly().GetName().Name.ToLower().Replace(".", "-")}-
            });
        }
       
    }   

    private static void SetMinimumLogLevel(LoggerConfiguration serilogConfig, string minLogLevel)
    {
        var loggingLevelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = minLogLevel.ToLower() switch
            {
                "debug" => LogEventLevel.Debug,
                "information" => LogEventLevel.Information,
                "warning" => LogEventLevel.Warning,
                _ => LogEventLevel.Information,
            }
        };
        serilogConfig.MinimumLevel.ControlledBy(loggingLevelSwitch);
    }

    private static void OverideMinimumLogLevel(LoggerConfiguration serilogConfig)
    {
        serilogConfig
                     .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                     .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                     .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                     .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
                     .MinimumLevel.Override("OpenIddict.Validation", LogEventLevel.Error)
                     .MinimumLevel.Override("System.Net.Http.HttpClient.OpenIddict", LogEventLevel.Error)
                     .MinimumLevel.Override("Microsoft.AspNetCore.HttpLogging.HttpLoggingMiddleware", LogEventLevel.Information);
    }
}
