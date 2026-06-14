using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using TodoWebApi.Common.Middleware;
using TodoWebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);


// Configure database and related services
builder.Services.AddDatabase(builder.Configuration, builder.Environment);
builder.Services.AddDatabaseSeeding(builder.Environment);

// Add application services
builder.Services.AddApplicationServices();

// Add services to the container
builder.Services.AddControllers();

// Configure OpenAPI/Swagger
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Version = "2.0";
        document.Info.Title = "Car Management API";
        document.Info.Description = "A clean .NET 9 API for managing cars and their owners with EF Core migrations.";
        document.Info.Contact = new OpenApiContact
        {
            Name = "Monirul Islam",
            Email = "mohammadmonirul@gmail.com",
            Url = new Uri("https://mmonirul.se")
        };
        document.Info.License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://opensource.org/licenses/MIT")
        };
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Car Management API v2.0");
    options.RoutePrefix = "swagger";
});
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
