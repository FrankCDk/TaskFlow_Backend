using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskFlow.Application.Interfaces;
using TaskFlow.Infrastructure.Middlewares;
using TaskFlow.Infrastructure.Repository;
using TaskFlow.Infrastructure.Repository.SqlServer;

namespace TaskFlow.Infrastructure.Configuration
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<ILogsRepository, LogsRepository>();

            services.AddScoped<ExceptionMiddleware>();
            services.AddScoped<SqlDatabase>(provider => {
                var configuration = provider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                return new SqlDatabase(connectionString);
            });

            return services;
        }



    }
}
