using Microsoft.EntityFrameworkCore;
using TodoWebApi.Models;

namespace TodoWebApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Car> Cars => Set<Car>();
    public DbSet<CarOwner> CarOwners => Set<CarOwner>();
}