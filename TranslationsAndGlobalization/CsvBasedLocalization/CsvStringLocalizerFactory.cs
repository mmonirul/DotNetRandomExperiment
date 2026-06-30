using Microsoft.Extensions.Localization;
using System.Globalization;

namespace CsvBasedLocalization;


public class CsvStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly Dictionary<string, Dictionary<string, string>> _resources;

    public CsvStringLocalizerFactory(string csvPath)
    {
        _resources = LoadCsv(csvPath);
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new CsvStringLocalizer(GetCulture(), _resources);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new CsvStringLocalizer(GetCulture(), _resources);
    }

    private string GetCulture()
    {
        return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
    }

    private static Dictionary<string, Dictionary<string, string>> LoadCsv(string path)
    {
        var lines = File.ReadAllLines(path);
        var headers = lines[0].Split(',').Skip(1).ToArray();

        var dict = new Dictionary<string, Dictionary<string, string>>();

        foreach (var line in lines.Skip(1))
        {
            var parts = line.Split(',');
            var key = parts[0];

            var translations = new Dictionary<string, string>();

            for (int i = 0; i < headers.Length; i++)
            {
                if (parts.Length > i + 1)
                    translations[headers[i]] = parts[i + 1];
            }

            dict[key] = translations;
        }

        return dict;
    }
}

