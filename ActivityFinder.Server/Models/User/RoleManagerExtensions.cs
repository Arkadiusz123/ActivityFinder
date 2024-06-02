using Microsoft.AspNetCore.Identity;
using System.Reflection;

namespace ActivityFinder.Server.Models.User
{
    public static class RoleManagerExtensions
    {
        public async static void UpdateRoles(this RoleManager<IdentityRole> roleManager)
        {
            Type t = typeof(UserRoles);
            FieldInfo[] roles = t.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo r in roles) 
            { 
                var role = r.GetValue(null)!.ToString();

                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
}
