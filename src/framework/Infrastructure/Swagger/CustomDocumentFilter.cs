namespace FastDelivery.Framework.Infrastructure.Swagger;
internal class CustomDocumentFilter : IDocumentFilter
{
    private readonly IConfiguration _configuration;
    private readonly ExcludedPathsService _excludedPathsService;
    public CustomDocumentFilter(IConfiguration configuration, ExcludedPathsService excludedPathsService)
    {
        _configuration = configuration;
        _excludedPathsService = excludedPathsService;
    }

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        string clientId = "clientId1"; // Example client ID
        var excludedPaths = _excludedPathsService.GetExcludedPaths(clientId);

        if (excludedPaths == null)
        {
            excludedPaths = new List<string>
            {
                "/api/private",
                "/api/secret"
            };
            // Set excluded paths in cache
            _excludedPathsService.SetExcludedPaths("clientId1", excludedPaths);
        }

        var pathsToRemove = swaggerDoc.Paths
            .Where(path => excludedPaths.Any(excludePath => path.Key.Contains(excludePath)))
            .ToList();

        foreach (var path in pathsToRemove)
        {
            swaggerDoc.Paths.Remove(path.Key);
        }
    }
}
