//using LocalizedContent.Models.Cards;

//namespace LocalizedContent.Models;

//using NHI.PortableText.Model;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//public class PolymorphicCardListConverter : JsonConverter<List<ICard<object>>>
//{
//    public override List<ICard<object>> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    {
//        if (reader.TokenType != JsonTokenType.StartArray)
//        {
//            throw new JsonException("Expected start of array");
//        }

//        var cards = new List<ICard<object>>();
//        while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
//        {
//            var card = ParseCard(ref reader, options);
//            if (card != null)
//            {
//                cards.Add(card);
//            }
//        }
//        return cards;
//    }

//    private static ICard<object>? ParseCard(ref Utf8JsonReader reader, JsonSerializerOptions options)
//    {
//        using var jsonDoc = JsonDocument.ParseValue(ref reader);

//        if (!jsonDoc.RootElement.TryGetProperty("cardType", out var typeProperty))
//        {
//            throw new JsonException("Card type not specified");
//        }

//        var cardType = typeProperty.ValueKind switch
//        {
//            JsonValueKind.String => Enum.Parse<CardType>(typeProperty.GetString()!),
//            JsonValueKind.Number => (CardType)typeProperty.GetInt32(),
//            _ => throw new JsonException("Invalid cardType format")
//        };

//        return cardType switch
//        {
//            CardType.Description => DeserializeCard<DescriptionCard, IList<BlockModel>>(jsonDoc, options) as ICard<object>,
//            CardType.Accreditation => DeserializeCard<AccreditationCard, List<Accreditation>>(jsonDoc, options) as ICard<object>,
//            CardType.ShortDescription => DeserializeCard<ShortDescriptionCard, string>(jsonDoc, options) as ICard<object>,
//            CardType.Image => DeserializeCard<ImageCard, IList<Image>>(jsonDoc, options) as ICard<object>,
//            CardType.ProgramFees => throw new NotImplementedException(),
//            _ => new UnknownCard(jsonDoc.RootElement.GetRawText()) as ICard<object>
//        };
//    }

//    public override void Write(Utf8JsonWriter writer, List<ICard<object>> cards, JsonSerializerOptions options)
//    {
//        writer.WriteStartArray();

//        foreach (var card in cards)
//        {
//            JsonSerializer.Serialize(writer, card, card.GetType(), options);
//        }

//        writer.WriteEndArray();
//    }

//    private static TCard DeserializeCard<TCard, TContent>(JsonDocument jsonDoc, JsonSerializerOptions options)
//        where TCard : ICard<TContent>
//    {
//        return JsonSerializer.Deserialize<TCard>(jsonDoc.RootElement.GetRawText(), options)
//            ?? throw new JsonException($"Failed to deserialize {typeof(TCard).Name}");
//    }
//}

//public class UnknownCard(string content) : ICard<string>
//{
//    public CardType CardType => CardType.ShortDescription;
//    public string Content { get; set; } = content;
//}
