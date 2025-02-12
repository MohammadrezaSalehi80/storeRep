using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Store.Application.Interfaces.Context;
using Store.Application.Interfaces.FacadPattern;
using Store.Application.Services.Common.GetCategory;
using Store.Application.Services.Common.GetMenu;
using Store.Application.Services.Product.FacadPattern;
using Store.Application.Services.Users.Command.Delete;
using Store.Application.Services.Users.Command.Register;
using Store.Application.Services.Users.Query.GetRoles;
using Store.Application.Services.Users.Query.GetUsers;
using Store.Application.Services.Users.Query.Login;
using Store.Persistance.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndPoint.Site
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
            services.AddScoped<IDataBaseContext, DatabaseContext>();
            services.AddScoped<IGetUsersServices, GetUsersServices>();
            services.AddScoped<IGetRoles, GetRoles>();
            services.AddScoped<ILoginService, LoginServices>();
            services.AddScoped<IRegisterUsersServices, RegisterUsersServices>();
            services.AddScoped<IProductFacad, ProductFacad>();
            services.AddScoped<IGetMenuServices, GetMenuServices>();
            services.AddScoped<IGetCategoryForSearchServices, GetCategoryForSearchServices>();

            //AddAuthentication
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(option =>
            {
                option.LoginPath = new PathString("/");
                option.ExpireTimeSpan= TimeSpan.FromMinutes(1);
            });

            services.AddScoped<IDeleteUsersServices, DeleteUsersServices>();

            //AddDbContext
            services.AddDbContext<DatabaseContext>(op =>
                op.UseSqlServer(Configuration["Data:Store"]));

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}/{id?}");

                    endpoints.MapControllerRoute(
                    name: "areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                });
        }
    }
}
