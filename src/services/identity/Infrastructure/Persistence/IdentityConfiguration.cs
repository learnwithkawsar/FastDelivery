using FastDelivery.Service.Identity.Domain.Users;
using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FastDelivery.Service.Identity.Infrastructure.Persistence;
internal class AppUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        const string IdentitySchemaName = "Identity";
        builder.IsMultiTenant();
        builder.ToTable("Users", IdentitySchemaName);
    }
}