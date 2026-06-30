using Microsoft.Extensions.Localization;
using System.Globalization;

namespace LocalizationExample.Services;

public class LocalizationService
{
    private readonly IStringLocalizer<LocalizationService> _localizer;
    private readonly IStringLocalizerFactory _localizerFactory;

    public LocalizationService(IStringLocalizer<LocalizationService> localizer, IStringLocalizerFactory localizerFactory)
    {
        _localizer = localizer;
        _localizerFactory = localizerFactory;
    }

    public void SetCulture(string cultureCode)
    {
        var culture = new CultureInfo(cultureCode);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
    }

    public string GetLocalizedString(string key, params object[] arguments)
    {
        if (arguments.Length > 0)
        {
            return _localizer[key, arguments];
        }
        return _localizer[key];
    }

    public string GetLocalizedStringForCulture(string key, string cultureCode, params object[] arguments)
    {
        var culture = new CultureInfo(cultureCode);
        var originalCulture = CultureInfo.CurrentCulture;
        var originalUICulture = CultureInfo.CurrentUICulture;

        try
        {
            CultureInfo.CurrentCulture = culture;
            CultureInfo.CurrentUICulture = culture;
            return GetLocalizedString(key, arguments);
        }
        finally
        {
            CultureInfo.CurrentCulture = originalCulture;
            CultureInfo.CurrentUICulture = originalUICulture;
        }
    }

    public void DisplayGreetings()
    {
        Console.WriteLine(_localizer["Welcome"]);
        Console.WriteLine(_localizer["CurrentTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")]);
        Console.WriteLine(_localizer["Goodbye"]);
        Console.WriteLine();
    }
}
