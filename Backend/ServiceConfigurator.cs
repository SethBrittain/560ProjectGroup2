using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data.Sql;
using System.Data.SqlClient;
using pidgin.services;

namespace pidgin
{
    public class ServiceConfigurator
    {
        public void Configure(IServiceCollection services)
        {
            string connString = Environment.GetEnvironmentVariable("ASPNETCORE_DATABASE_CONNECTION_STRING") ?? throw new Exception("Invalid Connection String");
            NpgsqlConnection conn = NpgsqlDataSource.Create(connString).CreateConnection();
            services.AddSingleton<NpgsqlConnection>(conn);

            services.AddScoped<IUserService, UserService>();
        }
    }
}