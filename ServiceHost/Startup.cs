using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Infrastructure.Core;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ServiceHost
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpContextAccessor();
            var connectionString = Configuration.GetConnectionString("AMContext");
            AccountConfiguration.Configure(services, connectionString);

            services.AddTransient<IEmailService, EmailService>();

            services.AddTransient<IFileUploader, FileUploader>();
            services.AddTransient<IAutenticateHelper, AuthenticateHelper>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, c =>
                {
                    c.LoginPath = new PathString("/Registration");
                    c.LogoutPath = new PathString("/Index");
                    c.AccessDeniedPath = new PathString("/AccessDenied");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminArea", builder =>
                    builder.RequireRole(
                        new List<string> { AuthorizationRoles.Admin, AuthorizationRoles.ContentProducer }));
                options.AddPolicy("Inventory", builder =>
                    builder.RequireRole(AuthorizationRoles.Admin));
            });

            services.AddRazorPages();
            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Administrator", "/", "AdminArea");
                options.Conventions.AuthorizeAreaFolder("Administrator", "/Inventory", "Inventory");
            });
            // WEB API
            //.AddApplicationPart(typeof(ClassName).Assembly)
            //.AddApplicationPart(typeof(ClassName).Assembly);


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.Lax;
            });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseAuthentication();

            app.UseHttpsRedirection();

            app.UseCookiePolicy();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
