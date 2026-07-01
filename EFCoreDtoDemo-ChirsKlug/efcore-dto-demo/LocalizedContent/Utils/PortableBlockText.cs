using Newtonsoft.Json;
using NHI.PortableText;
using NHI.PortableText.Model;
using PortableText;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LocalizedContent.Utils;

public class PortableBlockText
{
    private readonly BlockConverter _blockConverter;

    public PortableBlockText()
    {
        _blockConverter = new BlockConverter();
    }

    public static PortableBlockText Create(string text)
    {
        var bc = new BlockConverter();
        var portableText = bc.SerializeHtml(text);


        var htmlcontent = PortableTextToHtml.Render(portableText);

        var portableText1 = bc.ConvertHtml(text);
        var stringContent = System.Text.Json.JsonSerializer.Serialize(portableText1, _serializerOptions);

        var pc2 = PortableTextToHtml.Render(stringContent);



        var portableTextStr = bc.SerializeHtml(text);



        return new PortableBlockText();
    }

    private static readonly JsonSerializerOptions _serializerOptions = new()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public static List<BlockModel> ToPortableText(string text)
    {
        var bc = new BlockConverter();
        if (string.IsNullOrEmpty(text))
        {
            text = "<p>Welcome to <b>PortableText</b></p>";
        }

        var portableText = bc.ConvertHtml(text);
        return portableText;
    }

    public string FromPortableText(List<BlockModel> blocks)
    {
        var settings = new BlockConverterSettings();
        return JsonConvert.SerializeObject(blocks, settings.JsonSerializerSettings);
    }
}
