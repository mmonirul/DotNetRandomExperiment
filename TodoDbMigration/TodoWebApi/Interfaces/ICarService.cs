using TodoWebApi.DTOs;

namespace TodoWebApi.Interfaces;

public interface ICarService
{
    Task<IEnumerable<CarDto>> GetAllCarsAsync();
    Task<CarDto> GetCarByIdAsync(int id);
    Task<CarDto> CreateCarAsync(CreateCarDto carDto);
    Task<CarDto> UpdateCarAsync(int id, UpdateCarDto carDto);
    Task<bool> DeleteCarAsync(int id);
    Task<IEnumerable<CarDto>> GetCarsByOwnerIdAsync(int ownerId);
}
