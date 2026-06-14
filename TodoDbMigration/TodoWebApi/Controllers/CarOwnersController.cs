using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoWebApi.Models;

namespace TodoWebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class CarOwnersController : ControllerBase
{
    private readonly AppDbContext _context;

    public CarOwnersController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all car owners
    /// </summary>
    /// <returns>List of all car owners</returns>
    [HttpGet]
    public async Task<IActionResult> GetCarOwners()
    {
        var carOwners = await _context.CarOwners.ToListAsync();
        return Ok(carOwners);
    }

    /// <summary>
    /// Get a specific car owner by ID
    /// </summary>
    /// <param name="id">Car owner ID</param>
    /// <returns>Car owner details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCarOwner(int id)
    {
        var carOwner = await _context.CarOwners.FindAsync(id);
        if (carOwner == null)
        {
            return NotFound($"Car owner with ID {id} not found.");
        }
        return Ok(carOwner);
    }

    /// <summary>
    /// Get car owner by Car ID
    /// </summary>
    /// <param name="carId">Car ID</param>
    /// <returns>Car owner for the specified car</returns>
    [HttpGet("by-car/{carId}")]
    public async Task<IActionResult> GetCarOwnerByCarId(int carId)
    {
        var carOwner = await _context.CarOwners
            .FirstOrDefaultAsync(co => co.CarId == carId);
        
        if (carOwner == null)
        {
            return NotFound($"No owner found for car with ID {carId}.");
        }
        return Ok(carOwner);
    }

    /// <summary>
    /// Create a new car owner
    /// </summary>
    /// <param name="carOwner">Car owner data</param>
    /// <returns>Created car owner</returns>
    [HttpPost]
    public async Task<IActionResult> CreateCarOwner([FromBody] CarOwner carOwner)
    {
        // Since [ApiController] handles null checking for us, we don't need explicit null check
        // Validate that the car exists
        var carExists = await _context.Cars.AnyAsync(c => c.Id == carOwner.CarId);
        if (!carExists)
        {
            return BadRequest($"Car with ID {carOwner.CarId} does not exist.");
        }

        // Check if car already has an owner
        var existingOwner = await _context.CarOwners
            .FirstOrDefaultAsync(co => co.CarId == carOwner.CarId);
        if (existingOwner != null)
        {
            return BadRequest($"Car with ID {carOwner.CarId} already has an owner.");
        }

        _context.CarOwners.Add(carOwner);
        await _context.SaveChangesAsync();
        
        return CreatedAtAction(nameof(GetCarOwner), new { id = carOwner.Id }, carOwner);
    }

    /// <summary>
    /// Update an existing car owner
    /// </summary>
    /// <param name="id">Car owner ID</param>
    /// <param name="carOwner">Updated car owner data</param>
    /// <returns>No content if successful</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCarOwner(int id, [FromBody] CarOwner carOwner)
    {
        if (id != carOwner.Id)
        {
            return BadRequest("Car owner ID mismatch.");
        }

        var existingCarOwner = await _context.CarOwners.FindAsync(id);
        if (existingCarOwner == null)
        {
            return NotFound($"Car owner with ID {id} not found.");
        }

        // Validate that the car exists if CarId is being changed
        if (existingCarOwner.CarId != carOwner.CarId)
        {
            var carExists = await _context.Cars.AnyAsync(c => c.Id == carOwner.CarId);
            if (!carExists)
            {
                return BadRequest($"Car with ID {carOwner.CarId} does not exist.");
            }

            // Check if the new car already has an owner
            var otherOwner = await _context.CarOwners
                .FirstOrDefaultAsync(co => co.CarId == carOwner.CarId && co.Id != id);
            if (otherOwner != null)
            {
                return BadRequest($"Car with ID {carOwner.CarId} already has an owner.");
            }
        }

        // Update the car owner properties
        existingCarOwner.FirstName = carOwner.FirstName;
        existingCarOwner.LastName = carOwner.LastName;
        existingCarOwner.Email = carOwner.Email;
        existingCarOwner.PhoneNumber = carOwner.PhoneNumber;
        existingCarOwner.FormattedAddress = carOwner.FormattedAddress;
        existingCarOwner.CarId = carOwner.CarId;

        _context.CarOwners.Update(existingCarOwner);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Delete a car owner
    /// </summary>
    /// <param name="id">Car owner ID</param>
    /// <returns>No content if successful</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCarOwner(int id)
    {
        var carOwner = await _context.CarOwners.FindAsync(id);
        if (carOwner == null)
        {
            return NotFound($"Car owner with ID {id} not found.");
        }

        _context.CarOwners.Remove(carOwner);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>
    /// Get all cars with their owners
    /// </summary>
    /// <returns>List of cars with owner information</returns>
    [HttpGet("cars-with-owners")]
    public async Task<IActionResult> GetCarsWithOwners()
    {
        var carsWithOwners = await _context.Cars
            .GroupJoin(_context.CarOwners,
                car => car.Id,
                owner => owner.CarId,
                (car, owners) => new
                {
                    Car = car,
                    Owner = owners.FirstOrDefault()
                })
            .ToListAsync();

        return Ok(carsWithOwners);
    }
}
