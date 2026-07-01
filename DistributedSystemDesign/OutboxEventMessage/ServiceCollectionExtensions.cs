using Microsoft.Extensions.DependencyInjection;
using OutboxEventMessage.Serialization;
using OutboxEventMessage.ServiceUtilities;

namespace OutboxEventMessage;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOutbox(this IServiceCollection services)
    {
        services.AddScoped<IOutbox, Outbox>();
        services.AddScoped<IRelay, Relay>();
        services.AddScoped<IOutboxInitializer, OutboxInitializer>();

        services.AddHostedService<PublishOutboxJob>();
        services.AddHostedService<CleanupOutboxJob>();

        services.AddSingleton<ISerializer, Serializer>();

        return services;
    }
}
