using LocalizedContent.Endpoints;
using LocalizedContent.Models.Cards;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.Converters.Add(new PolymorphicCardListConverter());
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        if (context.JsonTypeInfo.Type == typeof(IDictionary<CardType, ICard>))
        {
            schema.Type = "object";
            schema.AdditionalProperties = new OpenApiSchema
            {
                Type = "object"
            };
        }
        return Task.CompletedTask;
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Swagger"));
    app.MapScalarApiReference(options =>
    {
        options.WithTheme(ScalarTheme.Kepler).WithDarkModeToggle(true).WithClientButton(true);
    });
}

app.UseHttpsRedirection();

app.RegisterUserEndpoints();
app.RegisterCardSetsEndpoints();

app.Run();