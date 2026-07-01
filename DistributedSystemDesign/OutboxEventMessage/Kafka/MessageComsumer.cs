using Confluent.Kafka;

namespace OutboxEventMessage.Kafka;

internal class MessageComsumer(string topic, Dictionary<string, string> consumerConfig)
{
    private readonly IConsumer<string, string> _consumer = new ConsumerBuilder<string, string>(consumerConfig)
        .SetLogHandler((_, logMessage) => Console.WriteLine($"[{logMessage.Level}] {logMessage.Message}"))
        .SetErrorHandler((_, error) => Console.WriteLine($"Error: {error.Reason}"))
        .Build();

    public async Task StartAsync<TDataType>(Func<Message<string, string>, TDataType> translator, Action<TDataType> handler, CancellationToken cancellationToken = default)
    {
        try
        {
            _consumer.Subscribe(topic);
            while (true)
            {
                var consumeResult = _consumer.Consume(cancellationToken);
                if (consumeResult.IsPartitionEOF)
                {
                    await Task.Delay(10000, cancellationToken);
                    continue;
                }
                var translatedMessage = translator(consumeResult.Message);
                handler(translatedMessage);
            }
        }
        catch (OperationCanceledException)
        {
            _consumer.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        finally
        {
            _consumer.Dispose();
        }
    }
}
// https://www.youtube.com/watch?v=tIZC70Swfwk&ab_channel=NDCConferences


internal class TestConsumer
{
    public static async Task Execute()
    {
        const string topic = "test-topic";
        var consumer = new MessageComsumer("test-topic", new Dictionary<string, string>
        {
            { "group.id", "test-group" },
            { "bootstrap.servers", "localhost:9092" },
            { "auto.offset.reset", "earliest" }
        });

        CancellationTokenSource cts = new();
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            cts.Cancel();
        };

        await consumer.StartAsync<KafkaMessage>(translator, handler, cts.Token);
    }

    //Func<Message<string, string>, KafkaMessage> translator = message => { return new KafkaMessage(message.Topic, message.Value); };

    public static void handler(KafkaMessage message)
    {
        Console.WriteLine(message);
    }
    public static KafkaMessage translator(Message<string, string> message)
    {
        return new KafkaMessage(message.Key, message.Value);
    }

}

public record KafkaMessage(string Key, string Data);