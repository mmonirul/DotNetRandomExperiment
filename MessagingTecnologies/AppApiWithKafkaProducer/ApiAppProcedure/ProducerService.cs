using Confluent.Kafka;

namespace ApiAppProcedure;

public interface IProducerService
{
    Task<string> SendMessageAsync(string topic, string message);
}

public class ProducerService : IProducerService
{
    private readonly ProducerConfig _producerConfig;
    private readonly string _kafkaUrl;
    public ProducerService(IConfiguration configuration)
    {
        _kafkaUrl = configuration["Kafka:BootstrapServers"]
            ?? throw new ArgumentNullException("Kafka:BootstrapServers configuration is missing");
        _producerConfig = new ProducerConfig { BootstrapServers = _kafkaUrl };

    }
    public async Task<string> SendMessageAsync(string topic, string message)
    {
        try
        {
            var adminClientConfig = new AdminClientConfig
            {
                BootstrapServers = _kafkaUrl
            };
            using var adminClient = new AdminClientBuilder(adminClientConfig).Build();
            //var metadata = adminClient.GetMetadata(topic, TimeSpan.FromSeconds(10));
            //if (metadata.Topics.Count == 0)
            //{
            //    await adminClient.CreateTopicsAsync([new TopicSpecification { Name = topic, NumPartitions = 1, ReplicationFactor = 1 }]);
            //}
            var msg = new Message<Null, string> { Value = message };

            using var producer = new ProducerBuilder<Null, string>(_producerConfig).Build();
            await producer.ProduceAsync(topic, msg);

            producer.Flush(TimeSpan.FromMinutes(1));
            _producerConfig.RetryBackoffMs = 1000;
            _producerConfig.MessageTimeoutMs = 5000;
            _producerConfig.EnableIdempotence = true;

            return $"Message sent to kafka topic {topic} successfully";

        }
        catch (Exception ex)
        {
            throw;
        }
    }
}