using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Areas.Locations.Data.Services;
using WebStorageSystem.Areas.Products.Data.Services;
using WebStorageSystem.Data;
using WebStorageSystem.Data.Entities.Identities;

namespace WebStorageSystem
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
            // DATABASE
            services.AddMyDatabaseConfiguration(Configuration.GetConnectionString("LocalDb"));

            // IDENTITY
            services.AddMyIdentityConfiguration();

            // HTTPS
            //services.AddMyHttpsConfiguration();

            // COOKIE POLICY TODO: Use?
            //services.AddMyCookiePolicyConfiguration();

            // MVC/API SETTINGS
            services.AddAutoMapper(typeof(Startup));
            services.AddMyMvcConfiguration();

            // SERVICES FOR DB QUERIES
            services.AddMyDbCommunicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                //app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            //app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Areas",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
    public static class ServiceExtensions
    {
        public static void AddMyMvcConfiguration(this IServiceCollection services)
        {
            
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.AddControllersWithViews(options =>
                {
                    //TODO: Add Filters
                    //options.Filters.Add<>()
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.AllowTrailingCommas = true; // Allows for trailing commas in JSON file
                })
                .AddXmlSerializerFormatters(); // Adds XML serializer for input and output

            services.AddRazorPages(); // Identity Core Scaffolding uses Razor Pages
        }

        public static void AddMyDatabaseConfiguration(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.EnableSensitiveDataLogging(true); // TODO: Delete
                options.UseLoggerFactory(LoggerFactory.Create(builder => builder.AddConsole()));
                options.UseSqlServer(connectionString);
            });
        }

        public static void AddMyIdentityConfiguration(this IServiceCollection services)
        {
            /*
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            */

            //TODO: Add Claims/Roles

            services.AddDefaultIdentity<ApplicationUser>(options =>
                {
                    // Password settings
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;

                    // Lockout settings
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    options.Lockout.AllowedForNewUsers = true;
                })
                .AddEntityFrameworkStores<AppDbContext>();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(10);

                options.LoginPath = "/Identity/Account/Login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });
        }

        public static void AddMyHttpsConfiguration(this IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromMinutes(30); //TODO: Change when in long run production
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 443;
            });
        }

        public static void AddMyCookiePolicyConfiguration(this IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
        }

        public static void AddMyDbCommunicationServices(this IServiceCollection services)
        {
            // Location Services
            services.AddScoped<LocationTypeService>();
            services.AddScoped<LocationService>();

            // Product Services
            services.AddScoped<ManufacturerService>();
            services.AddScoped<ProductTypeService>();
            services.AddScoped<ProductService>();
            services.AddScoped<VendorService>();
            services.AddScoped<UnitService>();
            services.AddScoped<BundleService>();
        }
    }
}
