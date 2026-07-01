using TransactionalOutboxKafka.Domain;

namespace TransactionalOutboxKafka.Services;

public interface IInvoiceRepository
{
    Task AddEntityAsync(Invoice invoice, CancellationToken cancellationToken);
    Task SaveChangesAsync(CancellationToken token);
}
