using System.Security.Claims;
using Hein.Data;
using Hein.Data.Models;
using Hein.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace Hein
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = builder.Configuration;

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Auth";
                    options.LogoutPath = "/Auth/SignOut";
                })
                .AddGoogle(options =>
                {
                    options.ClientId = configuration["Authentication:Google:ClientId"];
                    options.ClientSecret = configuration["Authentication:Google:ClientSecret"];
                });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Auth";
                // TODO: Do the access denied page
                options.AccessDeniedPath = "";
            });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                                   throw new InvalidOperationException(
                                       "Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddControllersWithViews();

            builder.Services.AddAuthorization();

            // builder.Services.AddAuthentication().AddCookie(IdentityConstants.ApplicationScheme)
            // .AddBearerToken(IdentityConstants.BearerScheme);

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            builder.Services.AddRazorPages();

            var app = builder.Build();

            // Seed roles
            app.SeedRolesAsync().GetAwaiter().GetResult();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
                app.ApplyMigrations();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            // app.MapGet("/Cart", async (ClaimsPrincipal claims, ApplicationDbContext context) =>
            // {
            //     string userId = claims.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            //     return await context.Users.FindAsync(userId);
            // })
            // .RequireAuthorization();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Add session middleware before authentication
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapRazorPages();

            app.Run();
        }
    }
}