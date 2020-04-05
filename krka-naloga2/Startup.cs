using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using krka_naloga2.Data;
using krka_naloga2.Shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace krka_naloga2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<KrkaDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddDefaultIdentity<Uporabnik>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<KrkaDbContext>();

            services.AddTransient<IKrkaRepo, KrkaRepo>();
            services.AddTransient<IDostavaDataManager, DostavaDataManager>();
            services.AddTransient<IModelValidator, ModelValidator>();

            services.AddControllersWithViews();
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            UserManager<Uporabnik> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            SeedIdentity(userManager, roleManager);
        }

        private void SeedIdentity(UserManager<Uporabnik> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Roles
            if (!roleManager.RoleExistsAsync("Admin").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                };
                var roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Uporabnik").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Uporabnik",
                    NormalizedName = "UPORABNIK"
                };
                var roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Skladiscnik").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Skladiscnik",
                    NormalizedName = "SKLADISCNIK"
                };
                var roleResult = roleManager.CreateAsync(role).Result;
            }
            if (!roleManager.RoleExistsAsync("Test").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Test",
                    NormalizedName = "TEST"
                };
                var roleResult = roleManager.CreateAsync(role).Result;
            }

            //Test Users
            //ADMIN
            if (userManager.FindByNameAsync("admin@krka.si").Result == null)
            {
                var user = new Uporabnik();
                user.UserName = "admin@krka.si";
                user.Email = "admin@krka.si";
                user.PodjetjeId = 1;
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password123.").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Admin").Wait();
                }
            }
            //UPORABNIKI
            if (userManager.FindByNameAsync("user@bayer.com").Result == null)
            {
                var user = new Uporabnik();
                user.UserName = "user@bayer.com";
                user.Email = "user@bayer.com";
                user.PodjetjeId = 2;
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password123.").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Uporabnik").Wait();
                }
            }
            if (userManager.FindByNameAsync("user@lek.si").Result == null)
            {
                var user = new Uporabnik();
                user.UserName = "user@lek.si";
                user.Email = "user@lek.si";
                user.PodjetjeId = 3;
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password123.").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Uporabnik").Wait();
                }
            }
            //SKLADIŠÈNIK
            if (userManager.FindByNameAsync("sklad@krka.si").Result == null)
            {
                var user = new Uporabnik();
                user.UserName = "sklad@krka.si";
                user.Email = "sklad@krka.si";
                user.PodjetjeId = 1;
                user.EmailConfirmed = true;

                var result = userManager.CreateAsync(user, "Password123.").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Skladiscnik").Wait();
                }
            }
        }
    }
}
