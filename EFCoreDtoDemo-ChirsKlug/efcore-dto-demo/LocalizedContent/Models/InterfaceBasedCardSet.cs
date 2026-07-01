using LocalizedContent.Models.Cards;
using System.Text.Json.Serialization;

namespace LocalizedContent.Models;

public class InterfaceBasedCardSet
{
    public int Id { get; set; }

    public int OwnerId { get; set; }

    public string Locale { get; set; } = "en-US";

    [JsonConverter(typeof(PolymorphicCardListConverter))]
    public List<ICard> Cards { get; set; } = new();

    public static InterfaceBasedCardSet Create(int ownerId, string locale = "en-US") => new() { OwnerId = ownerId, Locale = locale };

    public void AddCard(ICard card)
    {
        if (Cards.Any(c => c.CardType == card.CardType))
        {
            throw new ArgumentException($"Card of type {card.CardType} already exists.");
        }

        Cards.Add(card);
    }
}
