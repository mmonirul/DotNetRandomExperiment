using Microsoft.EntityFrameworkCore;
using TodoWebApi.Interfaces;
using TodoWebApi.Models;

namespace TodoWebApi.Repositories;

public class CarRepository : BaseRepository<Car>, ICarRepository
{
    public CarRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Car>> GetCarsByOwnerIdAsync(int ownerId)
    {
        return await _dbSet
            .Where(c => c.CarOwnerId == ownerId)
            .ToListAsync();
    }

    public async Task<Car?> GetCarWithOwnerAsync(int id)
    {
        return await _dbSet
            .Include(c => c.CarOwner)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
}
