using RabbitMQ.Client;
using System.Text;

namespace RabbitMQProducerApp;

internal class RabbitMQProducer
{
    private readonly ConnectionFactory _connectionFactory;

    public RabbitMQProducer()
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = ConnectionFactory.DefaultUser,
            Password = ConnectionFactory.DefaultPass,
            Port = AmqpTcpEndpoint.UseDefaultPort
        };

    }
    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "my_message_queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

        var body = Encoding.UTF8.GetBytes(message);
        await channel.BasicPublishAsync(exchange: "", routingKey: "my_message_queue", body: body);

        Console.WriteLine($" [x] Sent {message}");
    }
}
