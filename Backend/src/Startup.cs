using Microsoft.AspNetCore.Authentication.Cookies;
using Npgsql;
using Pidgin.Model;
using Pidgin.Repository;
using Pidgin.Services;

namespace Pidgin;

public class Startup
{
    public IConfiguration Configuration { get; }
	private bool isDev = false;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
		if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
		{
			Environment.SetEnvironmentVariable("ASPNETCORE_AUTH_SERVICE_HOST", "http://localhost:4200/auth/cas/");
			Environment.SetEnvironmentVariable("ASPNETCORE_AUTH_CAS_HOST", "https://signin.k-state.edu/WebISO/");
			Environment.SetEnvironmentVariable("ASPNETCORE_CONNECTION_STRING", "Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=pidgin");
		}
        services.AddControllers();

		string connString = Environment.GetEnvironmentVariable("ASPNETCORE_CONNECTION_STRING") 
			?? throw new Exception("ASPNETCORE_CONNECTION_STRING environment variable not set");

        NpgsqlDataSource conn = NpgsqlDataSource.Create(connString);
        services.AddSingleton(conn);

        // Add custom services
		services.AddScoped<IObjectRepository<Channel>, ChannelRepository>();
		services.AddScoped<IObjectRepository<ChannelMessage>, ChannelMessageRepository>();
		services.AddScoped<IObjectRepository<DirectMessage>, DirectMessageRepository>();
		services.AddScoped<IObjectRepository<Organization>, OrganizationRepository>();
		services.AddScoped<IObjectRepository<User>, UserRepository>();
		services.AddScoped<IAnalyticsService, AnalyticsService>();
		services.AddScoped<IPasswordService, PasswordService>();

        // WebSocketManager manager = new(new ChannelMessageRepository(conn), new DirectMessageRepository(conn), new UserRepository(conn));
        // services.AddSingleton(manager);

        // Add cookie authentication scheme
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
        {
            // options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

            options.LoginPath = "/unauthorized";
            options.AccessDeniedPath = "/unauthorized";
            options.SlidingExpiration = true;
        });

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

        // services.AddEndpointsApiExplorer();
        // services.AddSwaggerGen();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
		Console.WriteLine("Environment: " + env.EnvironmentName + " Is Dev: " + env.IsDevelopment());

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

        // app.UseSwagger();
        // app.UseSwaggerUI();

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
