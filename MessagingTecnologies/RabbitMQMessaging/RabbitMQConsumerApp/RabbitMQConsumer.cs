using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RabbitMQConsumerApp;

internal class RabbitMQConsumer
{
    private readonly ConnectionFactory _connectionFactory;
    public RabbitMQConsumer()
    {
        _connectionFactory = new ConnectionFactory
        {
            HostName = "localhost",
            UserName = ConnectionFactory.DefaultUser,
            Password = ConnectionFactory.DefaultPass,
            Port = AmqpTcpEndpoint.UseDefaultPort
        };
    }

    public async Task ConsumeMessagesAsync()
    {
        using var connection = await _connectionFactory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(queue: "my_message_queue", durable: true, exclusive: false, autoDelete: false,
            arguments: null);

        Console.WriteLine(" [*] Waiting for messages.");

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
            await Task.Yield();
        };

        await channel.BasicConsumeAsync("my_message_queue", autoAck: true, consumer: consumer);

        Console.WriteLine("Consumer is waiting for messages. Press [enter] to stop.");
        Console.ReadLine();
    }
}
