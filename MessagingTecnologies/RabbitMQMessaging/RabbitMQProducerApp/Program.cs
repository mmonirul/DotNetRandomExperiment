using RabbitMQProducerApp;

string? message;
var producer = new RabbitMQProducer();
do
{
    Console.WriteLine("Send a message to RabbitMQ");
    message = Console.ReadLine();
    await producer.SendMessageAsync(message);

} while (!string.Equals(message, "exit", StringComparison.CurrentCultureIgnoreCase));