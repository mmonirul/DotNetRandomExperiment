using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System.Collections;
using System.Globalization;
using System.Resources;
using System.Text.Json;

//CultureInfo.CurrentCulture = new CultureInfo("fr");
//CultureInfo.CurrentUICulture = new CultureInfo("fr");

//var services = new ServiceCollection();

//services.AddSingleton<IStringLocalizerFactory>(sp => new CsvStringLocalizerFactory("Localization/translations.csv"));
//services.AddSingleton<IStringLocalizer>(sp =>
//{
//    var factory = sp.GetRequiredService<IStringLocalizerFactory>();
//    return factory.Create(null);
//});

//// Fix: Add the missing extension method by including the Microsoft.Extensions.DependencyInjection namespace
//var provider = services.BuildServiceProvider();

//var localizer = provider.GetRequiredService<IStringLocalizer>();

//Console.WriteLine(localizer["Hello"]);     // → Bonjour (if culture is fr)
//Console.WriteLine(localizer["Goodbye"]);

public class Program
{
    private static void Main(string[] args)
    {
        var baseDir = AppContext.BaseDirectory;

        // <-- Use your folder here -->
        var localesDir = Path.Combine(baseDir, "LocalizedJson");
        var resourcesDir = Path.Combine(baseDir, "Resources");

        Directory.CreateDirectory(resourcesDir);

        var baseResourceName = "Messages";

        // Generate missing .resx files from your JSON files
        foreach (var jsonFile in Directory.GetFiles(localesDir, "*.json"))
        {
            var culture = Path.GetFileNameWithoutExtension(jsonFile); // en, sv-SE, etc.
            var suffix = culture == "en" ? "" : $".{culture}";

            var resxFile = Path.Combine(resourcesDir, $"{baseResourceName}{suffix}.resx");

            GenerateResxFromJson(jsonFile, resxFile);
        }

        var services = new ServiceCollection();
        services.AddLocalization(options => options.ResourcesPath = "Resources");
        services.AddLogging();
        var provider = services.BuildServiceProvider();
        var localizerFactory = provider.GetRequiredService<IStringLocalizerFactory>();
        var localizer = localizerFactory.Create(baseResourceName, typeof(Program).Assembly.GetName().Name);

        foreach (var cultureName in new[] { "en", "sv-SE" })
        {
            CultureInfo.CurrentUICulture = new CultureInfo(cultureName);

            var resxFile = Path.Combine(resourcesDir, $"{baseResourceName}{(cultureName == "en" ? "" : $".{cultureName}")}.resx");

            Console.WriteLine($"Culture: {cultureName}");
            if (File.Exists(resxFile))
            {
                using (ResourceReader resxReader = new ResourceReader(resxFile))
                {
                    foreach (DictionaryEntry entry in resxReader)
                    {
                        Console.WriteLine($"{entry.Key} - {entry.Value}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"No resource file found for culture: {cultureName}");
            }

            Console.WriteLine($"Greeting: {localizer["Greeting"]}");
            Console.WriteLine($"Farewell: {localizer["Farewell"]}");
            Console.WriteLine();
        }
        Console.ReadKey();
    }

    static void GenerateResxFromJson(string jsonFilePath, string resxFilePath)
    {
        if (File.Exists(resxFilePath))
            return;

        var jsonString = File.ReadAllText(jsonFilePath);
        var data = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonString)
                   ?? new Dictionary<string, string>();

        // Fix for CS0246: Ensure ResXResourceWriter is properly referenced
        using var writer = new ResourceWriter(resxFilePath);
        foreach (var kvp in data)
        {
            writer.AddResource(kvp.Key, kvp.Value);
        }
        writer.Generate();

        Console.WriteLine($"Generated {Path.GetFileName(resxFilePath)} from {Path.GetFileName(jsonFilePath)}");
    }
}