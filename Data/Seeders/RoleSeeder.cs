using Microsoft.AspNetCore.Identity;

namespace Hein.Data.Seeders
{
    public static class RoleSeeder
    {
        public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            // Define default roles
            string[] roleNames = { "Admin", "Employee", "Customer" };

            foreach (var roleName in roleNames)
            {
                // Check if the role already exists
                var roleExists = await roleManager.RoleExistsAsync(roleName);
                
                if (!roleExists)
                {
                    // Create the role if it doesn't exist
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
