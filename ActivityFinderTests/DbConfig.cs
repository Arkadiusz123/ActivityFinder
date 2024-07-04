using ActivityFinder.Server.Database;
using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityFinderTests
{
    public class DbConfig
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public DbConfig(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public async Task InitializeData(IEnumerable<Activity> activities = null)
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
                var user = await SeedTestUser(userManager, roleManager);

                if (activities?.Count() > 0) 
                {
                    foreach (var entity in activities)
                    {
                        entity.Creator = user;
                        db.Add(entity);
                    }
                    db.SaveChanges();
                }
            }
        }

        private async Task<ApplicationUser> SeedTestUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Add roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Add user
            var testUser = new ApplicationUser { UserName = "testuser", Email = "testuser@example.com" };
            var userResult = await userManager.CreateAsync(testUser, "Testtest@123");

            if (userResult.Succeeded)
            {
                // Assign roles to user
                await userManager.AddToRoleAsync(testUser, "User");
                await userManager.AddToRoleAsync(testUser, "Admin");
            }
            return testUser;
        }
    }
}
