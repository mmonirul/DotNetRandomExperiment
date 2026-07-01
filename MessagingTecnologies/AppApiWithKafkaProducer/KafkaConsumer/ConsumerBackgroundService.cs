using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace KafkaConsumer;

public class ConsumerBackgroundService : BackgroundService
{
    private readonly IConsumer<Ignore, string> _consumer;
    private readonly ILogger<ConsumerBackgroundService> _logger;

    public ConsumerBackgroundService(IConfiguration configuration, ILogger<ConsumerBackgroundService> logger)
    {
        _logger = logger;
        var KafkaUrl = "localhost:9092";
        var consumerConfig = new ConsumerConfig
        {
            BootstrapServers = KafkaUrl,
            GroupId = "InventoryConsumerGroup",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        _consumer = new ConsumerBuilder<Ignore, string>(consumerConfig).Build();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var kafkTopic = "hello-kafka";
        _consumer.Subscribe(kafkTopic);

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Run(() => ProcessKafkaMessage(stoppingToken), stoppingToken);
            await Task.Delay(10, stoppingToken);
        }

        _consumer.Close();
    }

    public void ProcessKafkaMessage(CancellationToken stoppingToken)
    {
        try
        {
            var consumeResult = _consumer.Consume(stoppingToken);
            var message = consumeResult.Message.Value;
            _logger.LogInformation(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Kafka message");
        }
    }
}
