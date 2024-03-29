using FastDelivery.Service.Identity.Domain.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FastDelivery.Service.Identity.Infrastructure.Persistence
{
    public class IndentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public IndentityDbContext(DbContextOptions<IndentityDbContext> options):base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        }
    }
}
