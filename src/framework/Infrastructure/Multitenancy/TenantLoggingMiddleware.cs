using Finbuckle.MultiTenant.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace FastDelivery.Framework.Infrastructure.Multitenancy;
public class TenantLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<TenantLoggingMiddleware> _logger;
    private readonly IMultiTenantContextAccessor<AppTenantInfo> _tenantContextAccessor;

    public TenantLoggingMiddleware(RequestDelegate next, ILogger<TenantLoggingMiddleware> logger, IMultiTenantContextAccessor<AppTenantInfo> tenantContextAccessor)
    {
        _next = next;
        _logger = logger;
        _tenantContextAccessor = tenantContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        await _next(context);

        var tenantContext = _tenantContextAccessor.MultiTenantContext;
        if (tenantContext != null)
        {
            _logger.LogInformation($"Tenant resolved: {tenantContext.TenantInfo.Id}, Strategy: {tenantContext.StrategyInfo.StrategyType.Name}");
        }
        else
        {
            _logger.LogInformation("Tenant not resolved.");
        }
    }
}
