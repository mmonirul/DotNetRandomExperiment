namespace TodoWebApi.DTOs;

public class CarDto
{
    public int Id { get; set; }
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int? CarOwnerId { get; set; }
}

public class CreateCarDto
{
    public string Manufacturer { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }
    public decimal Price { get; set; }
    public int? CarOwnerId { get; set; }
}

public class UpdateCarDto
{
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public int? Year { get; set; }
    public decimal? Price { get; set; }
    public int? CarOwnerId { get; set; }
}
