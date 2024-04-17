using FastDelivery.Service.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FastDelivery.Service.Identity.Infrastructure.Persistence
{
    public class AppIndentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppIndentityDbContext(DbContextOptions<AppIndentityDbContext> options):base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
