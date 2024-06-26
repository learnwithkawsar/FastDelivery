﻿using FastDelivery.Framework.Infrastructure.Options;

namespace FastDelivery.Framework.Infrastructure.Swagger;
public class SwaggerOptions : IOptionsRoot
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public List<string> ExcludePaths { get; set; } = new List<string>();
}
