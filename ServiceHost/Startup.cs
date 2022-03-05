using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using _0_Framework.Application;
using _0_Framework.Application.Email;
using AM.Application;
using AM.Infrastructure.Core;
using AM.Management.API;
using Microsoft.AspNetCore.Authentication.Cookies;
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
        private readonly string _corsPolicy = "CorsPolicy";
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            var connectionString = Configuration.GetConnectionString("AMContext");
            AccountConfiguration.Configure(services, connectionString);
            services.AddTransient<IEmailService<EmailModel>, EmailService>();
            services.AddTransient<IFileUploader, FileUploader>();
            services.AddSingleton<IPasswordHasher, PasswordHasher>();
            services.AddTransient<IAuthenticateHelper, AuthenticateHelper>();
            services.AddSignalR(hubOptions =>
            {
                hubOptions.EnableDetailedErrors = true;
                hubOptions.MaximumParallelInvocationsPerClient = 2;
            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, c =>
                {
                    c.LogoutPath = new Microsoft.AspNetCore.Http.PathString("/Index");
                    c.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/AccessDenied");
                    c.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Authentication/Login");
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("DashboardArea", builder =>
                    builder.RequireRole(
                        new List<string>
                        {
                            AuthorizationRoles.Admin, AuthorizationRoles.CustomerofRawMaterial,
                            AuthorizationRoles.Plant, AuthorizationRoles.SupplierofRawMaterial,
                            AuthorizationRoles.TechnologyProvider
                        }));
                options.AddPolicy("AdminArea", builder =>
                    builder.RequireRole(AuthorizationRoles.Admin));
            });

            // services.AddRazorPages().WithRazorPagesRoot("/Index");
            services.AddCors(options =>
            {
                options.AddPolicy(name: _corsPolicy, builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddRazorPages().AddRazorPagesOptions(options =>
            {
                options.Conventions.AuthorizeAreaFolder("Dashboard", "/", "DashboardArea");
                options.Conventions.AuthorizeAreaFolder("Dashboard", "/Users", "AdminArea");
                options.Conventions.AuthorizeAreaFolder("Dashboard", "/ContactUs", "AdminArea");
            })
            .AddApplicationPart(typeof(NotificationController).Assembly);


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
            });

            services.Configure<CookieTempDataProviderOptions>(options =>
            {
                options.Cookie.IsEssential = true;
            });

            services.AddRouting(options => options.LowercaseUrls = true);


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
                // app.UseHsts();
            }
            // app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseAuthentication();


            app.Use(async (context, next) =>
            {
                await next();
                if (context.Response.StatusCode == 404)
                {
                    context.Request.Path = "/Shared/_PageNotFound";
                    await next();
                }
            });
            app.UseRouting();
            app.UseCors(_corsPolicy);
            app.UseWebSockets();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHub<NotificationHub>("/notificationHub");
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
