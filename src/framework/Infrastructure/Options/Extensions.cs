﻿using FastDelivery.Framework.Core.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Infrastructure.Options;
public static class Extensions
{
    public static T LoadOptions<T>(this IConfiguration configuration, string sectionName) where T : IOptionsRoot
    {
        var options = configuration.GetSection(sectionName).Get<T>() ?? throw new ConfigurationMissingException(sectionName);
        return options;
    }

    public static T BindValidateReturn<T>(this IServiceCollection services, IConfiguration configuration) where T : class, IOptionsRoot
    {
        services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        return configuration.LoadOptions<T>(typeof(T).Name);
    }
    public static void BindValidate<T>(this IServiceCollection services) where T : class, IOptionsRoot
    {
        services.AddOptions<T>()
            .BindConfiguration(typeof(T).Name)
            .ValidateDataAnnotations()
            .ValidateOnStart();
    }
}