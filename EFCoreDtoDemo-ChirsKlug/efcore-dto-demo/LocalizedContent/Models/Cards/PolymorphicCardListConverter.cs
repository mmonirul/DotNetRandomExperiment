namespace LocalizedContent.Models.Cards;

using System.Text.Json;
using System.Text.Json.Serialization;

public class PolymorphicCardListConverter : JsonConverter<List<ICard>>
{
    public override List<ICard> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray) throw new JsonException("Expected start of array");

        var cards = new List<ICard>();
        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
        {
            var card = ParseCard(ref reader, options);
            if (card != null)
            {
                cards.Add(card);
            }
        }

        return cards;
    }

    private static ICard? ParseCard(ref Utf8JsonReader reader, JsonSerializerOptions options)
    {
        using var jsonDoc = JsonDocument.ParseValue(ref reader);

        if (!jsonDoc.RootElement.TryGetProperty("cardType", out var typeProperty))
        {
            throw new JsonException("Card type not specified");
        }

        var cardType = typeProperty.ValueKind switch
        {
            JsonValueKind.String => Enum.Parse<CardType>(typeProperty.GetString()!),
            JsonValueKind.Number => (CardType)typeProperty.GetInt32(),
            _ => throw new JsonException("Invalid cardType format")
        };

        return cardType switch
        {
            CardType.Description => DeserializeCard<DescriptionCard>(jsonDoc, options),
            CardType.Accreditation => DeserializeCard<AccreditationCard>(jsonDoc, options),
            CardType.ShortDescription => DeserializeCard<ShortDescriptionCard>(jsonDoc, options),
            CardType.Image => DeserializeCard<ImageCard>(jsonDoc, options),
            CardType.ProgramFees => throw new NotImplementedException(),
            _ => new UnknownCard(jsonDoc.RootElement.GetRawText())
        };
    }

    public override void Write(Utf8JsonWriter writer, List<ICard> cards, JsonSerializerOptions options)
    {
        writer.WriteStartArray();

        var customOptions = new JsonSerializerOptions(options)
        {
            Converters = { new JsonStringEnumConverter() }
        };
        cards.ForEach(card => JsonSerializer.Serialize(writer, card, card.GetType(), customOptions));

        writer.WriteEndArray();
    }

    private static T DeserializeCard<T>(JsonDocument jsonDoc, JsonSerializerOptions options) where T : ICard
    {
        return JsonSerializer.Deserialize<T>(jsonDoc.RootElement.GetRawText(), options)
            ?? throw new JsonException($"Failed to deserialize {nameof(T)}");
    }
}
public record UnknownCard(string RawJson, CardType CardType = CardType.Unknown) : ICard;
