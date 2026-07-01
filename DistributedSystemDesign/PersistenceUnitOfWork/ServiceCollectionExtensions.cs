using Microsoft.Extensions.DependencyInjection;

namespace PersistenceUnitOfWork;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddUnitOfWork(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
