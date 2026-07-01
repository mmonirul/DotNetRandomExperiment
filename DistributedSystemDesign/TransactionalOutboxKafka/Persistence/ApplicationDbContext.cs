using Microsoft.EntityFrameworkCore;
using TransactionalOutboxKafka.Domain;

namespace TransactionalOutboxKafka.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(SafeAssemblySearchAncestor).Assembly);
    }
}

public record SafeAssemblySearchAncestor;