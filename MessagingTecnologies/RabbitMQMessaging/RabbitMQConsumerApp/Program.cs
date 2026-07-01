
using RabbitMQConsumerApp;

var consumer = new RabbitMQConsumer();

await consumer.ConsumeMessagesAsync();