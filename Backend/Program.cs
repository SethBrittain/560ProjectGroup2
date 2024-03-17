using System.Text;
using Npgsql;
using pidgin.services;
using pidgin.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

string connString = "Host=postgres;Username=postgres;Password=postgres;Database=pidgin;Port=5432;";
//Environment.GetEnvironmentVariable("ASPNETCORE_DATABASE_CONNECTION_STRING") ?? throw new Exception("Invalid Connection String");
NpgsqlDataSource conn = NpgsqlDataSource.Create(connString);

builder.Services.AddSingleton(conn);

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddScoped<IChannelService, ChannelService>();
builder.Services.AddScoped<IOrganizationService, OrganizationService>();

// Configure password requirements and options
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = true;
});

// Configure cookie settings for tracking authentication between requests
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

    options.LoginPath = "/auth/cas/login";
    options.AccessDeniedPath = "/error";
    options.SlidingExpiration = true;
});

// Add the configured application cookie as an authentication scheme
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie("Cookies");

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

var webSocketOptions = new WebSocketOptions()
{
	KeepAliveInterval = TimeSpan.FromSeconds(120)
};
app.UseWebSockets(webSocketOptions);

app.UseAuthorization();
app.MapControllers();

app.MapFallbackToFile("index.html");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();

	app.MapGet("/debug/routes", (IEnumerable<EndpointDataSource> endpointSources) =>
		string.Join("\n", endpointSources.SelectMany(source => source.Endpoints)));
}
app.Run();
