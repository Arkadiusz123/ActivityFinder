using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ActivityFinder.Server.Database
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<Comment> Comments { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Activity>().HasOne(a => a.Creator).WithMany(a => a.CreatedActivities).HasForeignKey(a => a.CreatorId);
            builder.Entity<Comment>().HasOne(a => a.User).WithMany(a => a.Comments).HasForeignKey(a => a.UserId);

            base.OnModelCreating(builder);
        }
    }
}
