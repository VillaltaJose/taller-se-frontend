using Backend.Application.Services;
using Backend.Application.Services.Tickets;
using Backend.Core.Interfaces.Repositories;
using Backend.Core.Interfaces.Services;
using Backend.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbProvider(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DataBase") ?? "";
            //services.AddScoped<IDbConnection>((sp) => new NpgsqlConnection(connectionString));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IAuthenticationRepository, AuthenticationRepository>();
            services.AddScoped<ITicketRepository, TicketRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IPasswordService, PasswordService>();
            services.AddTransient<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ITicketService, TicketService>();

            return services;
        }
    }
}
