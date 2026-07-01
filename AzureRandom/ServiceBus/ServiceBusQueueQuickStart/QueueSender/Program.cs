
using Azure.Messaging.ServiceBus;

public class Program
{
    public static async Task Main(string[] args)
    {
        var connectionString = "Endpoint=sb://az204exsbusns.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=cznxP5nohD5xlBisdCOewZm+xaZ90tlbS+ASbIDbel0=";

        ServiceBusSender serviceBusSender;
        const int numberOfMessages = 5;

        var clientOptions = new ServiceBusClientOptions
        {
            TransportType = ServiceBusTransportType.AmqpWebSockets
        };
        var serviceBusClient = new ServiceBusClient(connectionString, clientOptions);

        serviceBusSender = serviceBusClient.CreateSender("ContosoOrdersQueue");

        using var messageBatch = await serviceBusSender.CreateMessageBatchAsync();

        for (var i = 0; i < numberOfMessages; i++)
        {
            var message = new ServiceBusMessage($"Order {i}");
            if (!messageBatch.TryAddMessage(message))
            {
                throw new System.InvalidOperationException($"The message {i} is too large to fit in the batch.");
            }
        }

        try
        {
            await serviceBusSender.SendMessagesAsync(messageBatch);
            Console.WriteLine($"A batch of {numberOfMessages} messages has been published to the queue.");
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            await serviceBusSender.DisposeAsync();
            await serviceBusClient.DisposeAsync();
        }

        Console.WriteLine("Press any key to end the application");
        Console.ReadKey();
    }
}