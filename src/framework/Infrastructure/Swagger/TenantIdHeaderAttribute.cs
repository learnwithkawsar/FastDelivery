namespace FastDelivery.Framework.Infrastructure.Swagger;
public class TenantIdHeaderAttribute : SwaggerHeaderAttribute
{
    public TenantIdHeaderAttribute()
        : base(
            "tenant",
            "Input your tenant Id to access this API",
            string.Empty,
            true)
    {
    }
}