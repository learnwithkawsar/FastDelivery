using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Infrastructure.Swagger;
public class ExcludedPathsService
{
    private readonly IMemoryCache _cache;

    public ExcludedPathsService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public void SetExcludedPaths(string clientId, List<string> excludedPaths)
    {
        // Store excluded paths in cache
        _cache.Set(clientId, excludedPaths, TimeSpan.FromHours(1));
    }

    public List<string> GetExcludedPaths(string clientId)
    {
        // Retrieve excluded paths from cache
        return _cache.Get<List<string>>(clientId);
    }
}
