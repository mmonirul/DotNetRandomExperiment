using Microsoft.Extensions.Localization;
using System.Globalization;
using System.Text.Json;

namespace LocalizerWorkerService;

// JsonStringLocalizerFactory.cs
public class JsonStringLocalizerFactory : IStringLocalizerFactory
{
    private readonly string _resourcesPath;

    public JsonStringLocalizerFactory(string resourcesPath)
    {
        _resourcesPath = resourcesPath;
    }

    public IStringLocalizer Create(Type resourceSource)
    {
        return new JsonStringLocalizer(_resourcesPath, resourceSource.Name);
    }

    public IStringLocalizer Create(string baseName, string location)
    {
        return new JsonStringLocalizer(_resourcesPath, baseName);
    }
}

public class JsonStringLocalizer : IStringLocalizer
{
    private readonly string _resourcesPath;
    private readonly string _baseName;

    public JsonStringLocalizer(string resourcesPath, string baseName)
    {
        _resourcesPath = resourcesPath;
        _baseName = baseName;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetString(name);
            var value = string.Format(format ?? name, arguments);
            return new LocalizedString(name, value, resourceNotFound: format == null);
        }
    }

    private string? GetString(string name)
    {
        var currentCulture = CultureInfo.CurrentCulture.Name;

        var cultureFilePath = Path.Combine(_resourcesPath, $"resources.{currentCulture}.json");
        var neutralFilePath = Path.Combine(_resourcesPath, "resources.en.json");

        var filePath = File.Exists(cultureFilePath) ? cultureFilePath : neutralFilePath;

        if (File.Exists(filePath))
        {
            var jsonContent = File.ReadAllText(filePath);
            var resourceData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);

            if (resourceData != null && resourceData.TryGetValue(name, out var value))
            {
                return value;
            }
        }

        return null;
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        // Implement if needed
        throw new NotImplementedException();
    }

    public Dictionary<string, string> GetAllTranslations(string key)
    {
        var translations = new Dictionary<string, string>();
        var supportedCultures = new[] { "en", "de-GE", "sv-SE" }; // Your supported cultures

        foreach (var culture in supportedCultures)
        {
            var previousCulture = CultureInfo.CurrentCulture; // Save the current culture
            CultureInfo.CurrentCulture = new CultureInfo(culture); // Set the culture

            var localizedValue = this[key]; // Use the indexer to get the localized value
            if (!localizedValue.ResourceNotFound)
            {
                translations.Add(culture, localizedValue.Value);
            }

            CultureInfo.CurrentCulture = previousCulture; // Restore the previous culture
        }

        return translations;
    }

}
