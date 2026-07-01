namespace TransactionalOutboxKafka.Models;

public sealed class InvoiceModel
{
    public decimal Amount { get; set; }
    public DateTime DueDate { get; set; }
}
public sealed class InvoicePayload
{
    public Guid Id { get; set; }
}

