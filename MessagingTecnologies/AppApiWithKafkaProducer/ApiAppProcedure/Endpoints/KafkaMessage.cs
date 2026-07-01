using Microsoft.OpenApi.Models;
using System.Text.Json;

namespace ApiAppProcedure.Endpoints;

public static class KafkaMessage
{
    public static void RegisterKafkaMessageEndpoint(this IEndpointRouteBuilder routes)
    {
        var kafkaMessage = routes.MapGroup("/api/v1/message");

        kafkaMessage.MapPost("send", async (IProducerService producerService, string topic, User user) =>
        {
            var result = await producerService.SendMessageAsync(topic, JsonSerializer.Serialize(user));
            return Results.Ok(result);
        }).WithOpenApi(x => new OpenApiOperation(x)
        {
            Summary = "Send message to kafka stream",
            Description = "Send message to kafka stream",
            Tags = [new OpenApiTag { Name = "Kafka messaging" }]
        });

        //app.MapPost("kafkaProducer", async (string topic, object message) =>
        //{
        //    var producerService = app.Services.GetRequiredService<IProducerService>();
        //    var result = await producerService.SendMessageAsync(topic, JsonSerializer.Serialize(message));
        //    return Results.Ok(result);
        //})
        //.WithName("KafkaProducer")
        //.WithOpenApi(x => new OpenApiOperation(x)
        //{
        //    Summary = "Send message to kafka stream",
        //    Description = "Send message to kafka stream",
        //    Tags = [new() { Name = "Kafka messaging" }]
        //});
    }
}

public class User
{
    public int Id { get; init; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    internal DateOnly BirthDate { get; set; }
}