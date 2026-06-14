using TodoWebApi.Common.Exceptions;
using TodoWebApi.DTOs;
using TodoWebApi.Interfaces;
using TodoWebApi.Models;

namespace TodoWebApi.Services;

public class CarService : ICarService
{
    private readonly ICarRepository _carRepository;

    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<IEnumerable<CarDto>> GetAllCarsAsync()
    {
        var cars = await _carRepository.GetAllAsync();
        return cars.Select(car => MapToDto(car));
    }

    public async Task<CarDto> GetCarByIdAsync(int id)
    {
        var car = await _carRepository.GetCarWithOwnerAsync(id);
        if (car == null)
        {
            throw new NotFoundException($"Car with ID {id} not found.");
        }
        return MapToDto(car);
    }

    public async Task<CarDto> CreateCarAsync(CreateCarDto carDto)
    {
        if (carDto == null)
        {
            throw new ApiException("Car data cannot be null.", 400);
        }

        var car = new Car
        {
            Manufacturer = carDto.Manufacturer,
            Model = carDto.Model,
            Year = carDto.Year,
            Price = carDto.Price,
            CarOwnerId = carDto.CarOwnerId ?? 0
        };

        var createdCar = await _carRepository.AddAsync(car);
        await _carRepository.SaveChangesAsync();
        return MapToDto(createdCar);
    }

    public async Task<CarDto> UpdateCarAsync(int id, UpdateCarDto carDto)
    {
        if (carDto == null)
        {
            throw new ApiException("Update data cannot be null.", 400);
        }

        var car = await _carRepository.GetByIdAsync(id);
        if (car == null)
        {
            throw new NotFoundException($"Car with ID {id} not found.");
        }

        if (carDto.Manufacturer != null) car.Manufacturer = carDto.Manufacturer;
        if (carDto.Model != null) car.Model = carDto.Model;
        if (carDto.Year.HasValue) car.Year = carDto.Year.Value;
        if (carDto.Price.HasValue) car.Price = carDto.Price.Value;
        if (carDto.CarOwnerId.HasValue) car.CarOwnerId = carDto.CarOwnerId.Value;

        await _carRepository.UpdateAsync(car);
        await _carRepository.SaveChangesAsync();
        return MapToDto(car);
    }

    public async Task<bool> DeleteCarAsync(int id)
    {
        var car = await _carRepository.GetByIdAsync(id);
        if (car == null)
        {
            throw new NotFoundException($"Car with ID {id} not found.");
        }

        await _carRepository.DeleteAsync(car);
        await _carRepository.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<CarDto>> GetCarsByOwnerIdAsync(int ownerId)
    {
        var cars = await _carRepository.GetCarsByOwnerIdAsync(ownerId);
        return cars.Select(car => MapToDto(car));
    }

    private static CarDto MapToDto(Car car)
    {
        return new CarDto
        {
            Id = car.Id,
            Manufacturer = car.Manufacturer,
            Model = car.Model,
            Year = car.Year,
            Price = car.Price,
            CarOwnerId = car.CarOwnerId
        };
    }
}
