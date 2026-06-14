using System.ComponentModel.DataAnnotations;

namespace TodoWebApi.Models;

public class Car
{
    public int Id { get; set; }
    
    [Required]
    public string Manufacturer { get; set; } = string.Empty;
    
    [Required]
    public string Model { get; set; } = string.Empty;
    
    public int Year { get; set; }
    
    [Required]
    public decimal Price { get; set; }
    
    public int CarOwnerId { get; set; }
    
    public CarOwner? CarOwner { get; set; }
}
