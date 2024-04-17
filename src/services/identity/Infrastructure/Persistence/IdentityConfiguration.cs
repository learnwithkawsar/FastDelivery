using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FastDelivery.Service.Identity.Domain.Users;

namespace FastDelivery.Service.Identity.Infrastructure.Persistence;
internal class AppUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        const string IdentitySchemaName = "Identity";
        builder.ToTable("Users", IdentitySchemaName);
    }
}