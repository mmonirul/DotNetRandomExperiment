using TodoWebApi.Models;

namespace TodoWebApi.Interfaces;

public interface ICarRepository : IRepository<Car>
{
    Task<IEnumerable<Car>> GetCarsByOwnerIdAsync(int ownerId);
    Task<Car?> GetCarWithOwnerAsync(int id);
}
