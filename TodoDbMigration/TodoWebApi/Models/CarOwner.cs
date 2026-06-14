using System.ComponentModel.DataAnnotations;

namespace TodoWebApi.Models;

public class CarOwner
{
    public int Id { get; set; }
    
    [Required]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    public string LastName { get; set; } = string.Empty;
    
    [Required]
    public string Email { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    [Required]
    public string FormattedAddress { get; set; } = string.Empty;
    
    public int CarId { get; set; }
    public Car? Car { get; set; }

    public string Name => $"{FirstName} {LastName}";
}
