using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using pidgin.services;

namespace Pidgin;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // Add database connection services
        string connString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTION_STRING")
            ?? "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=conferenceplanner";

        NpgsqlDataSource conn = NpgsqlDataSource.Create(connString);
        services.AddSingleton(conn);

        // Add custom services


        // Configure identity settings
        //services.Configure<IdentityOptions>(options =>
        //{
        //    // Password settings.
        //    options.Password.RequireDigit = true;
        //    options.Password.RequireLowercase = true;
        //    options.Password.RequireNonAlphanumeric = true;
        //    options.Password.RequireUppercase = true;
        //    options.Password.RequiredLength = 8;
        //    options.Password.RequiredUniqueChars = 1;

        //    // Lockout settings.
        //    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        //    options.Lockout.MaxFailedAccessAttempts = 5;
        //    options.Lockout.AllowedForNewUsers = true;

        //    // User settings.
        //    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
        //    options.User.RequireUniqueEmail = true;
        //});

        // Configure cookie settings
        //services.ConfigureApplicationCookie(options =>
        //{
        //    options.Cookie.HttpOnly = true;
        //    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

        //    options.LoginPath = "/authcas/login";
        //    options.AccessDeniedPath = "/error";
        //    options.SlidingExpiration = true;
        //});

        // Add cookie authentication scheme
        //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie("Cookies");
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Allow serving static files from wwwroot
        app.UseStaticFiles();

        // Enables routing for controllers
        app.UseRouting();

        // Enables secure pages and authentication
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            // Maps the /debug/routes endpoint if in development environment
            if (env.IsDevelopment())
                endpoints.MapGet("/debug/routes",
                    (IEnumerable<EndpointDataSource> endpointSources) =>
                        string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)
                    )
                );

            // Maps controllers to routes from attributes on the controller classes and methods
            endpoints.MapControllers();

            // If no routes match, serve index.html. Necessary for SPA routing to work.
            endpoints.MapFallbackToFile("index.html");
        });
    }
}