using Microsoft.EntityFrameworkCore;
using TodoWebApi.Models;

namespace TodoWebApi.Services;

public class DatabaseSeederService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<DatabaseSeederService> _logger;

    public DatabaseSeederService(IServiceProvider serviceProvider, ILogger<DatabaseSeederService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        try
        {
            _logger.LogInformation("Starting database seeding...");
            
            // Ensure database is created
            await context.Database.EnsureCreatedAsync(cancellationToken);
            
            // Seed demo data
            await SeedDemoDataAsync(context, cancellationToken);
            
            _logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task SeedDemoDataAsync(AppDbContext context, CancellationToken cancellationToken)
    {
        // Seed demo car if it doesn't exist
        var demoCar = await context.Cars.FirstOrDefaultAsync(c => c.Id == 101, cancellationToken);
        if (demoCar == null)
        {
            context.Cars.Add(new Car 
            { 
                Id = 101, 
                Manufacturer = "Tesla", 
                Model = "Model S", 
                Year = 2022,
                Price = 89990
            });
            
            await context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Demo car seeded successfully");
        }
    }
}
