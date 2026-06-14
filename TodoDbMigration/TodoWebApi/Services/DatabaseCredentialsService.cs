using System.Text.Json;
using Amazon;
using Amazon.Runtime;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;

namespace TodoWebApi.Services;

public class DatabaseCredentialsService : IDatabaseCredentialsService
{
    private readonly IAmazonSecretsManager _secretsManagerClient;
    private readonly ILogger<DatabaseCredentialsService> _logger;

    public DatabaseCredentialsService(IAmazonSecretsManager secretsManagerClient, ILogger<DatabaseCredentialsService> logger)
    {
        _secretsManagerClient = secretsManagerClient;
        _logger = logger;
    }

    public async Task<string> GetConnectionStringAsync(string secretName, string databaseName, string hostName, int portNumber = 5432)
    {
        var secretResponse = await _secretsManagerClient.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT",
        });

        string secretString = secretResponse.SecretString;
        var secret = JsonSerializer.Deserialize<Dictionary<string, string>>(secretString);
        if (secret is null) throw new KeyNotFoundException($"Secret Name {secretName} Not Found or is Empty.");

        var username = secret["username"];
        var password = secret["password"];
        var database = databaseName;
        var host = hostName;
        var port = portNumber;

        // Construct the PostgreSQL connection string
        var connectionString = $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        _logger.LogInformation("Connection String: {ConnectionString}", connectionString);
        return connectionString;
    }

    public static async Task<string> GetConnectionStringAsync()
    {

        string secretName = "rds!db-ad7ccb96-a3bc-4e65-a335-bb23aad57d88";
        string region = "us-east-1";

        var credentials = new BasicAWSCredentials("CRED_NAME", "CRED_KEY");
        var client = new AmazonSecretsManagerClient(credentials, RegionEndpoint.GetBySystemName(region));

        GetSecretValueRequest request = new GetSecretValueRequest
        {
            SecretId = secretName,
            VersionStage = "AWSCURRENT",
        };

        GetSecretValueResponse response;

        try
        {
            response = await client.GetSecretValueAsync(request);
        }
        catch (Exception e)
        {
            // For a list of the exceptions thrown, see
            // https://docs.aws.amazon.com/secretsmanager/latest/apireference/API_GetSecretValue.html
            throw;
        }

        return response.SecretString;
    }

}