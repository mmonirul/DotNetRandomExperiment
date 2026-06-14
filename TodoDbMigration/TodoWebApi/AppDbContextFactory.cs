using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TodoWebApi;

/// <summary>
/// Design-time factory for creating DbContext instances for EF Core tooling operations.
/// This factory is used by EF Core CLI tools (migrations, scaffolding, etc.)
/// and provides a simple way to create DbContext without complex application startup.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        // Use local development connection string for EF tooling operations
        // This matches the connection string in appsettings.Development.json
        var connectionString = "Host=localhost;Port=5432;Database=car-management;Username=postgres;Password=localpassword;Include Error Detail=true";
        
        optionsBuilder.UseNpgsql(connectionString);
        
        return new AppDbContext(optionsBuilder.Options);
    }
}
