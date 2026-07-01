
using Microsoft.EntityFrameworkCore;
using OutboxEventMessage;
using OutboxEventMessage.Kafka;
using PersistenceUnitOfWork;
using TransactionalOutboxKafka.Events;
using TransactionalOutboxKafka.Persistence;
using TransactionalOutboxKafka.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DbConnectionString");
builder.Services.AddPersistence<ApplicationDbContext>(connectionString ?? throw new ArgumentException("The database configuration string is missing"));

builder.Services.AddOutbox();
builder.Services.AddKafka(InvoiceCreatedEvent.Topic);

builder.Services.AddUnitOfWork();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
await dbContext.Database.MigrateAsync(CancellationToken.None);

app.UseHttpsRedirection();

app.UseAuthorization();


app.MapControllers();

app.Run();

//https://github.com/alex-popov-stenn/SimpleTO/tree/main/src
//https://dev.to/fairday/implementing-horizontally-scalable-transactional-outbox-pattern-with-net-8-and-kafka-a-practical-guide-1l1
