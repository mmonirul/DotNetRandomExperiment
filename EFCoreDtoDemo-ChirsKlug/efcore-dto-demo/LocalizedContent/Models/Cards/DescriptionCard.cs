using NHI.PortableText.Model;

namespace LocalizedContent.Models.Cards;

public interface ICard
{
    CardType CardType { get; }
}
public interface ICard<out TContent> : ICard
{
    TContent Content { get; }
}

public enum CardType
{
    Accreditation,
    Description,
    ShortDescription,
    Image,
    ProgramFees,
    Unknown
}

public class DescriptionCard : ICard<IList<BlockModel>>
{
    public CardType CardType => CardType.Description;
    public IList<BlockModel> Content { get; set; } = new List<BlockModel>();
}

public class AccreditationCard : ICard<List<Accreditation>>
{
    public CardType CardType => CardType.Accreditation;
    public List<Accreditation> Content { get; set; } = new List<Accreditation>();
}

public class ShortDescriptionCard : ICard<string>
{
    public CardType CardType => CardType.ShortDescription;
    public string Content { get; set; } = string.Empty;
}

public class ImageCard : ICard<IList<Image>>
{
    public CardType CardType => CardType.Image;
    public IList<Image> Content { get; set; } = new List<Image>();
}

