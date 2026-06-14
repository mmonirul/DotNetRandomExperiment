using Microsoft.AspNetCore.Mvc;
using TodoWebApi.Common;
using TodoWebApi.DTOs;
using TodoWebApi.Interfaces;

namespace TodoWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CarsController : ControllerBase
{
    private readonly ICarService _carService;

    public CarsController(ICarService carService)
    {
        _carService = carService;
    }

    /// <summary>
    /// Get all cars
    /// </summary>
    /// <returns>List of all cars</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<CarDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCars()
    {
        var cars = await _carService.GetAllCarsAsync();
        return Ok(new ApiResponse<IEnumerable<CarDto>>
        {
            Data = cars,
            Message = "Cars retrieved successfully"
        });
    }

    /// <summary>
    /// Get a specific car by ID
    /// </summary>
    /// <param name="id">Car ID</param>
    /// <returns>Car details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<CarDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCar(int id)
    {
        var car = await _carService.GetCarByIdAsync(id);
        return Ok(new ApiResponse<CarDto>
        {
            Data = car,
            Message = "Car retrieved successfully"
        });
    }

    /// <summary>
    /// Create a new car
    /// </summary>
    /// <param name="carDto">Car details</param>
    /// <returns>Created car</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CarDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCar([FromBody] CreateCarDto carDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse
            {
                Status = 400,
                Message = "Invalid input",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        var createdCar = await _carService.CreateCarAsync(carDto);
        var response = new ApiResponse<CarDto>
        {
            Status = 201,
            Data = createdCar,
            Message = "Car created successfully"
        };

        return CreatedAtAction(nameof(GetCar), new { id = createdCar.Id }, response);
    }

    /// <summary>
    /// Update an existing car
    /// </summary>
    /// <param name="id">Car ID</param>
    /// <param name="carDto">Updated car details</param>
    /// <returns>No content if successful</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateCar(int id, [FromBody] UpdateCarDto carDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiResponse
            {
                Status = 400,
                Message = "Invalid input",
                Errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
            });
        }

        await _carService.UpdateCarAsync(id, carDto);
        return NoContent();
    }

    /// <summary>
    /// Delete a car
    /// </summary>
    /// <param name="id">Car ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCar(int id)
    {
        await _carService.DeleteCarAsync(id);
        return NoContent();
    }
}
