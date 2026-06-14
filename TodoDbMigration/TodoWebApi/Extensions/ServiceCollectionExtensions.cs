using Microsoft.EntityFrameworkCore;
using TodoWebApi.Configuration;
using TodoWebApi.Interfaces;
using TodoWebApi.Repositories;
using TodoWebApi.Services;

namespace TodoWebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
    {
        // Configure database options
        services.Configure<DatabaseOptions>(configuration.GetSection(DatabaseOptions.SectionName));

        if (environment.IsDevelopment())
        {
            // Local development - use connection string from appsettings.Development.json
            services.AddDbContext<AppDbContext>(options =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("DefaultConnection string is not configured for Development environment.");

                options.UseNpgsql(connectionString);

                // Enable detailed logging for development
                var databaseOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>();
                if (databaseOptions?.EnableSensitiveDataLogging == true)
                {
                    options.EnableSensitiveDataLogging();
                }
                if (databaseOptions?.EnableDetailedErrors == true)
                {
                    options.EnableDetailedErrors();
                }
            });
        }
        else
        {
            // Production - use Azure App Service configuration
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException(
                    "DefaultConnection string must be configured in Azure App Service Configuration. " +
                    "Please add a connection string named 'DefaultConnection' in Azure Portal > App Service > Configuration > Connection strings.");
            }

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
                
                // Add logging for production debugging if needed
                var databaseOptions = configuration.GetSection(DatabaseOptions.SectionName).Get<DatabaseOptions>();
                if (databaseOptions?.EnableDetailedErrors == true)
                {
                    options.EnableDetailedErrors();
                }
            });
        }

        return services;
    }

    public static IServiceCollection AddDatabaseSeeding(this IServiceCollection services, IWebHostEnvironment environment)
    {
        if (environment.IsDevelopment())
        {
            // Add seeding only in development
            services.AddHostedService<DatabaseSeederService>();
        }

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register repositories
        services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
        services.AddScoped<ICarRepository, CarRepository>();

        // Register services
        services.AddScoped<ICarService, CarService>();

        return services;
    }
}
