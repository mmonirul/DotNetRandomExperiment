using Microsoft.Extensions.Localization;
using System.Globalization;

namespace LocalizationExample.Services;


// Service to handle multi-culture localization
public class MultiCultureLocalizationService
{
    private readonly IStringLocalizerFactory _localizerFactory;

    public MultiCultureLocalizationService(IStringLocalizerFactory localizerFactory)
    {
        _localizerFactory = localizerFactory;
    }

    public string GetLocalizedString(string key, string cultureName, Type resourceType = null)
    {
        var culture = new CultureInfo(cultureName);
        var localizer = resourceType != null
            ? _localizerFactory.Create(resourceType)
            : _localizerFactory.Create("SharedResources", "YourApp.Resources");

        // Use WithCulture to get culture-specific localizer
        var cultureSpecificLocalizer = localizer.WithCulture(culture);
        return cultureSpecificLocalizer[key];
    }

    public Dictionary<string, string> GetLocalizedStrings(string[] keys, string cultureName, Type resourceType = null)
    {
        var culture = new CultureInfo(cultureName);
        var localizer = resourceType != null
            ? _localizerFactory.Create(resourceType)
            : _localizerFactory.Create("SharedResources", "YourApp.Resources");

        var cultureSpecificLocalizer = localizer.WithCulture(culture);

        return keys.ToDictionary(key => key, key => cultureSpecificLocalizer[key].Value);
    }

    public Dictionary<string, Dictionary<string, string>> GetMultiCultureStrings(string[] keys, string[] cultures, Type resourceType = null)
    {
        var result = new Dictionary<string, Dictionary<string, string>>();

        foreach (var cultureName in cultures)
        {
            result[cultureName] = GetLocalizedStrings(keys, cultureName, resourceType);
        }

        return result;
    }
}

// Extension method for easier usage
public static class StringLocalizerExtensions
{
    public static IStringLocalizer WithCulture(this IStringLocalizer localizer, CultureInfo culture)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;

            // Create a wrapper that maintains the culture context
            return new CultureSpecificStringLocalizer(localizer, culture);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }
}

// Culture-specific localizer wrapper
public class CultureSpecificStringLocalizer : IStringLocalizer
{
    private readonly IStringLocalizer _innerLocalizer;
    private readonly CultureInfo _culture;

    public CultureSpecificStringLocalizer(IStringLocalizer innerLocalizer, CultureInfo culture)
    {
        _innerLocalizer = innerLocalizer;
        _culture = culture;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentCulture = _culture;
                CultureInfo.CurrentUICulture = _culture;
                return _innerLocalizer[name];
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var originalCulture = CultureInfo.CurrentCulture;
            var originalUICulture = CultureInfo.CurrentUICulture;

            try
            {
                CultureInfo.CurrentCulture = _culture;
                CultureInfo.CurrentUICulture = _culture;
                return _innerLocalizer[name, arguments];
            }
            finally
            {
                CultureInfo.CurrentCulture = originalCulture;
                CultureInfo.CurrentUICulture = originalUICulture;
            }
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = _culture;
            CultureInfo.CurrentUICulture = _culture;
            return _innerLocalizer.GetAllStrings(includeParentCultures);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }
}
