using Microsoft.EntityFrameworkCore.Design;
using System.Diagnostics.CodeAnalysis;

namespace TransactionalOutboxKafka.Persistence;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
public sealed class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        var services = new ServiceCollection();
        services.AddPersistence<ApplicationDbContext>(configuration.GetConnectionString("DbConnectionString")!);
        var provider = services.BuildServiceProvider();

        return provider.GetRequiredService<ApplicationDbContext>();
    }
}
