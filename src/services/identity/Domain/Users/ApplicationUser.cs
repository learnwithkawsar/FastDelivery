using Microsoft.AspNetCore.Identity;

namespace FastDelivery.Service.Identity.Domain.Users;

public class ApplicationUser : IdentityUser
{
    public string? FullAddress { get; set; }
    public string? TenantId { get; set; }
}
