using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Npgsql;
using pidgin.services;
using Pidgin.Services;
using System.Diagnostics;

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
            ?? "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=pidgin";

        NpgsqlDataSource conn = NpgsqlDataSource.Create(connString);
        services.AddSingleton(conn);
        // Add custom services
		services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrganizationService, OrganizationService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<IChannelService, ChannelService>();

        Services.WebSocketManager manager = new(new MessageService(conn, new UserService(conn)));
        services.AddSingleton(manager);

        // Add cookie authentication scheme
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

            options.LoginPath = "/unauthorized";
            options.AccessDeniedPath = "/error";
            options.SlidingExpiration = true;
        });

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        // Allow websocket connection
        var webSocketOptions = new WebSocketOptions()
        {
            KeepAliveInterval = TimeSpan.FromSeconds(120),
        };
        app.UseWebSockets(webSocketOptions);

        // Allow serving static files from wwwroot
        app.UseStaticFiles();

        // Enables routing for controllers
        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI();

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