using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActivityFinder.Server.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Activity> Activity { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Activity>().HasOne(a => a.Creator).WithMany(a => a.CreatedActivities).HasForeignKey(a => a.CreatorId);

            base.OnModelCreating(builder);
        }
    }
}
