namespace TodoWebApi.Configuration;

public class DatabaseOptions
{
    public const string SectionName = "DatabaseOptions";

    public string Provider { get; set; } = "PostgreSQL";
    public string? DatabaseName { get; set; }
    public bool EnableSensitiveDataLogging { get; set; } = false;
    public bool EnableDetailedErrors { get; set; } = false;
}
