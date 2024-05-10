using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace FastDelivery.Framework.Infrastructure.Common;
public static class Extensions
{
    public static IServiceCollection AddCustomeApiVersioning(this IServiceCollection services) =>
      services.AddApiVersioning(config =>
      {
          config.DefaultApiVersion = new ApiVersion(1, 0);
          config.AssumeDefaultVersionWhenUnspecified = true;
          config.ReportApiVersions = true;
      });
}
