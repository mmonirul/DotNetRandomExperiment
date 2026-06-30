using Microsoft.Extensions.Localization;

namespace CsvBasedLocalization;


public class CsvStringLocalizer : IStringLocalizer
{
    private readonly string _culture;
    private readonly Dictionary<string, Dictionary<string, string>> _resources;

    public CsvStringLocalizer(string culture, Dictionary<string, Dictionary<string, string>> resources)
    {
        _culture = culture;
        _resources = resources;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = _resources.TryGetValue(name, out var translations) &&
                        translations.TryGetValue(_culture, out var translation)
                        ? translation
                        : name;

            return new LocalizedString(name, value, resourceNotFound: value == name);
        }
    }

    public LocalizedString this[string name, params object[] arguments] => this[name];

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        foreach (var kvp in _resources)
        {
            var value = kvp.Value.TryGetValue(_culture, out var translation)
                        ? translation
                        : kvp.Key;

            yield return new LocalizedString(kvp.Key, value);
        }
    }
}

