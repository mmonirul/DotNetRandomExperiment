using LocalizedContent.Models;
using LocalizedContent.Models.Cards;
using LocalizedContent.Utils;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.OpenApi.Models;
using PortableText;

namespace LocalizedContent.Endpoints;

public static class CardSetsEndpoints
{
    public static void RegisterCardSetsEndpoints(this WebApplication app)
    {
        app.MapGet("/cardsets", async Task<Results<Ok<InterfaceBasedCardSet>, NotFound>> () => await GetCardSet())
            .WithName("Getcardsets")
            .WithOpenApi(x => new OpenApiOperation(x)
            {
                Summary = "Get cards with metadata.",
                Description = "Returns information about localized content.",
                Tags = new List<OpenApiTag> { new() { Name = "Card sets" } }
            });

        app.MapPost("/cardsets", async Task<Results<Created<InterfaceBasedCardSet>, NotFound>> (InterfaceBasedCardSet cardSet) =>
        {

            if (cardSet == null)
            {
                return TypedResults.NotFound();
            }

            cardSet.Id = 100;

            return TypedResults.Created("1", cardSet);
        });
    }




    static async Task<Results<Ok<InterfaceBasedCardSet>, NotFound>> GetCardSet()
    {
        var cardSet = CreteNewCardSet();

        var pt = new PortableTextBlock();

        //var json = JsonSerializer.Serialize(cardSet, new JsonSerializerOptions
        //{
        //    WriteIndented = true,
        //});
        //var directoryPath = Path.Combine("./Data/CardSets");
        //if (!Directory.Exists(directoryPath))
        //{
        //    Directory.CreateDirectory(directoryPath);
        //}
        //var filePath = Path.Combine(directoryPath, $"{cardSet.Locale}.json");
        //await File.WriteAllTextAsync(filePath, json);

        var portableText = LocalizedContent.Utils.PortableBlockText.Create("<p>Welcome to <b>PortableText</b></p>");

        return TypedResults.Ok(cardSet);
    }

    static InterfaceBasedCardSet CreteNewCardSet()
    {
        var cardSet = InterfaceBasedCardSet.Create(1);
        cardSet.AddCard(new ShortDescriptionCard
        {
            Content = "This is a short description."
        });

        cardSet.AddCard(new DescriptionCard
        {
            Content = PortableBlockText.ToPortableText("<p>Welcome to <b>PortableText</b></p>")
        });

        cardSet.AddCard(new AccreditationCard
        {
            Content =
            [
                new Accreditation { Name = "Accreditation 1" },
                new Accreditation { Name = "Accreditation 2" }
            ]
        });

        return cardSet;
    }
}