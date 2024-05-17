using DetailingCenterApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DetApp3.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DetApp3.Areas.Identity.Data;
using System;

namespace DetApp3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DetailingCenterDBContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DetailingCenterDB")));
            //builder.Services.AddDefaultIdentity<DetApp3User>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<DetApp3Context>();
            
            // Add services to the container.
            //builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //options.UseSqlServer(builder.Configuration.GetConnectionString("MainBase")));
            builder.Services.AddDbContext<DetApp3Context>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DetApp3ContextConnection")));

            builder.Services.AddIdentity<DetApp3User, IdentityRole>(options =>
            options.SignIn.RequireConfirmedAccount = false).
             AddEntityFrameworkStores<DetApp3Context>();

            builder.Services.ConfigureApplicationCookie(opt =>
            {
                opt.AccessDeniedPath = new PathString("/Identity/Account/AccessDenied");
                opt.LoginPath = new PathString("/Identity/Account/Login");
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.MapRazorPages();
            app.Run();
        }
    }
}