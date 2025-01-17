using Microsoft.AspNetCore.Identity;
using Hein.Data.Seeders;

namespace Hein.Extensions
{
    public static class WebApplicationExtensions
    {
        public static async Task SeedRolesAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await RoleSeeder.SeedRoles(roleManager);
            }
        }
    }
}
