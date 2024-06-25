using ActivityFinder.Server.Database;
using ActivityFinder.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ActivityFinderTests
{
    internal class DbConfig
    {
        private readonly CustomWebApplicationFactory<Program> _factory;

        public DbConfig(CustomWebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public async Task InitializeData()
        {
            using (var scope = _factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<AppDbContext>();
                var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole>>();

                db.Database.EnsureCreated();
                await SeedTestUser(userManager, roleManager);
            }
        }

        private async Task SeedTestUser(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
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
        }
    }
}
