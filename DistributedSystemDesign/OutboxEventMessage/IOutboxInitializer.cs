namespace OutboxEventMessage;

public interface IOutboxInitializer
{
    Task InitializeAsync(CancellationToken cancellationToken);
}
