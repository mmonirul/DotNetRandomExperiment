using System.Collections;
using System.Globalization;
using System.Resources.NetStandard;

namespace JsonToResxLocalizationApp;
public class RuntimeResourceManager
{
    private readonly string _baseName;
    private readonly string _resourcePath;

    public RuntimeResourceManager(string baseName, string resourcePath = "Resources")
    {
        _baseName = baseName;
        _resourcePath = resourcePath;
    }

    public string GetString(string key, string culture = null)
    {
        culture = culture ?? CultureInfo.CurrentUICulture.Name;

        // Try specific culture first (e.g., sv-SE)
        string value = GetStringFromResx($"{_baseName}.{culture}.resx", key);
        if (value != null) return value;

        // Try neutral culture (e.g., sv)
        if (culture.Contains("-"))
        {
            string neutralCulture = culture.Split('-')[0];
            value = GetStringFromResx($"{_baseName}.{neutralCulture}.resx", key);
            if (value != null) return value;
        }

        // Try default culture
        value = GetStringFromResx($"{_baseName}.resx", key);
        return value ?? key;
    }

    private string GetStringFromResx(string fileName, string key)
    {
        string resxPath = Path.Combine(_resourcePath, fileName);

        if (!File.Exists(resxPath))
            return null;

        using (var reader = new ResXResourceReader(resxPath))
        {
            foreach (DictionaryEntry entry in reader)
            {
                if (entry.Key.ToString().Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return entry.Value?.ToString();
                }
            }
        }

        return null;
    }
}
