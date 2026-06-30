using System.Collections.Concurrent;
using System.Text.Json;

namespace LocalizerWorkerService;
public class TranslationService
{
    private readonly ConcurrentDictionary<string, Dictionary<string, string>> _allTranslations;

    public TranslationService()
    {
        _allTranslations = LoadAllTranslations();
    }

    public Dictionary<string, string> GetTranslationsForKey(string key)
        => _allTranslations.TryGetValue(key, out var translations) ? translations : [];

    private ConcurrentDictionary<string, Dictionary<string, string>> LoadAllTranslations()
    {
        var translations = new ConcurrentDictionary<string, Dictionary<string, string>>();

        // Define the supported cultures and resource file paths
        var supportedCultures = new[] { "en", "de-GE" }; // Add more cultures as needed
        var resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");

        foreach (var culture in supportedCultures)
        {
            var filePath = Path.Combine(resourcesPath, $"resources.{culture}.json");

            if (File.Exists(filePath))
            {
                var jsonContent = File.ReadAllText(filePath);
                var resourceData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);

                if (resourceData != null)
                {
                    foreach (var kvp in resourceData)
                    {
                        // Add or update the key in the dictionary
                        translations.AddOrUpdate(kvp.Key,
                            _ => new Dictionary<string, string> { { culture, kvp.Value } },
                            (_, existing) =>
                            {
                                existing[culture] = kvp.Value;
                                return existing;
                            });
                    }
                }
            }
        }

        return translations;
    }
}
