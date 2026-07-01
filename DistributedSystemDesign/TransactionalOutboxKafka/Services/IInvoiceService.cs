using TransactionalOutboxKafka.Models;

namespace TransactionalOutboxKafka.Services;

public interface IInvoiceService
{
    Task<Guid> CreateInvoice(InvoiceModel model, CancellationToken cancellationToken);
}

