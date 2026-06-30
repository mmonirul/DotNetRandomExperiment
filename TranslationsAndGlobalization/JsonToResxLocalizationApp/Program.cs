using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Resources;
using System.Resources.NetStandard;
using System.Text.Json;

namespace JsonToResxLocalizationApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("JSON to RESX Localization Demo");
            Console.WriteLine("==============================");

            // Step 1: Create RESX files from JSON files
            CreateResxFromJson();

            Console.WriteLine("Please build the project again to embed the RESX files, then run the executable.");
            Console.WriteLine("For now, let's demonstrate with ResourceManager directly...\n");

            // Step 2: Demonstrate direct ResourceManager usage (works immediately)
            DemonstrateWithResourceManager();

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        private static void CreateResxFromJson()
        {
            Console.WriteLine("Creating RESX files from JSON...");

            // Create Resources directory if it doesn't exist
            var resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
            Directory.CreateDirectory(resourcesPath);

            // Process English JSON
            var enJsonPath = "en.json";
            var enJson = GetEnglishJson(); // Using embedded JSON content
            CreateResxFile(enJson, Path.Combine(resourcesPath, "AppResources.resx"));
            Console.WriteLine("Created: AppResources.resx (English - default)");

            // Process Swedish JSON
            var svJsonPath = "sv-SE.json";
            var svJson = GetSwedishJson(); // Using embedded JSON content
            CreateResxFile(svJson, Path.Combine(resourcesPath, "AppResources.sv-SE.resx"));
            Console.WriteLine("Created: AppResources.sv-SE.resx (Swedish)");

            Console.WriteLine("RESX files created successfully!\n");
        }

        private static void CreateResxFile(string jsonContent, string resxPath)
        {
            try
            {
                // Parse JSON
                var jsonData = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonContent);

                // Create RESX writer using the NetStandard package
                using var writer = new ResXResourceWriter(resxPath);

                // Add each key-value pair to the RESX file
                foreach (var kvp in jsonData)
                {
                    writer.AddResource(kvp.Key, kvp.Value);
                }

                // Generate the RESX file
                writer.Generate();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating RESX file {resxPath}: {ex.Message}");
            }
        }

        private static void DemonstrateWithResourceManager()
        {
            Console.WriteLine("Demonstrating with ResourceManager (works immediately):");
            Console.WriteLine("------------------------------------------------------");

            try
            {
                // Test with English
                Console.WriteLine("English (en-US):");
                TestWithResourceManager("en-US");

                Console.WriteLine("\nSwedish (sv-SE):");
                TestWithResourceManager("sv-SE");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Note: The RESX files need to be embedded resources. Try building the project again.");
            }
        }

        private static void TestWithResourceManager(string cultureName)
        {

            try
            {
                var culture = new CultureInfo(cultureName);
                var resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");

                Console.WriteLine($"-----------------------------------------");
                Console.WriteLine($"{GetLocalizedString("WelcomeMessage", cultureName)}");
                Console.WriteLine($"-----------------------------------------");

                // Create ResourceManager pointing to our generated RESX files
                var ss = System.Reflection.Assembly.GetExecutingAssembly();
                var resourceManager = new ResourceManager("JsonToResxLocalizationApp.AppResources", System.Reflection.Assembly.GetExecutingAssembly());

                // Set the culture
                var originalCulture = CultureInfo.CurrentCulture;
                var originalUICulture = CultureInfo.CurrentUICulture;

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

                try
                {
                    Console.WriteLine($"  Greeting: {resourceManager.GetString("Greeting", culture) ?? "[Not Found]"}");
                    Console.WriteLine($"  Welcome Message: {resourceManager.GetString("WelcomeMessage", culture) ?? "[Not Found]"}");
                    Console.WriteLine($"  Date Format: {resourceManager.GetString("DateFormat", culture) ?? "[Not Found]"}");
                    Console.WriteLine($"  Thank You: {resourceManager.GetString("ThankYou", culture) ?? "[Not Found]"}");

                    var itemsFormat = resourceManager.GetString("ItemsCount", culture) ?? "You have {0} items";
                    Console.WriteLine($"  Items Count (5 items): {string.Format(itemsFormat, 5)}");

                    Console.WriteLine($"  Farewell: {resourceManager.GetString("Farewell", culture) ?? "[Not Found]"}");

                    var dateFormat = resourceManager.GetString("DateFormat", culture) ?? "yyyy-MM-dd";
                    Console.WriteLine($"  Current Date: {DateTime.Now.ToString(dateFormat)}");
                }
                finally
                {
                    // Restore original culture
                    CultureInfo.CurrentCulture = originalCulture;
                    CultureInfo.CurrentUICulture = originalUICulture;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error loading resources for {cultureName}: {ex.Message}");

                // Fallback: read directly from RESX files
                Console.WriteLine("  Falling back to direct RESX file reading...");
                ReadDirectlyFromResxFile(cultureName);
            }
        }

        private static void ReadDirectlyFromResxFile(string cultureName)
        {
            try
            {
                var resourcesPath = Path.Combine(Directory.GetCurrentDirectory(), "Resources");
                var fileName = cultureName == "en-US" ? "AppResources.resx" : $"AppResources.{cultureName}.resx";
                var filePath = Path.Combine(resourcesPath, fileName);

                if (File.Exists(filePath))
                {
                    using var reader = new ResXResourceReader(filePath);
                    var resources = reader.Cast<System.Collections.DictionaryEntry>().ToDictionary(
                        entry => entry.Key.ToString(),
                        entry => entry.Value.ToString()
                    );

                    Console.WriteLine($"  Greeting: {resources.GetValueOrDefault("Greeting", "[Not Found]")}");
                    Console.WriteLine($"  Welcome Message: {resources.GetValueOrDefault("WelcomeMessage", "[Not Found]")}");
                    Console.WriteLine($"  Date Format: {resources.GetValueOrDefault("DateFormat", "[Not Found]")}");
                    Console.WriteLine($"  Thank You: {resources.GetValueOrDefault("ThankYou", "[Not Found]")}");

                    var itemsFormat = resources.GetValueOrDefault("ItemsCount", "You have {0} items");
                    Console.WriteLine($"  Items Count (5 items): {string.Format(itemsFormat, 5)}");

                    Console.WriteLine($"  Farewell: {resources.GetValueOrDefault("Farewell", "[Not Found]")}");

                    var dateFormat = resources.GetValueOrDefault("DateFormat", "yyyy-MM-dd");
                    Console.WriteLine($"  Current Date: {DateTime.Now.ToString(dateFormat)}");
                }
                else
                {
                    Console.WriteLine($"  RESX file not found: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"  Error reading RESX file: {ex.Message}");
            }
        }

        public static string GetLocalizedString(string key, string culture)
        {
            var resourceManager = new RuntimeResourceManager("AppResources");
            string localizedText = resourceManager.GetString("WelcomeMessage", culture);

            return localizedText ?? $"[{key}]";
        }

        // Embedded JSON content (simulating the uploaded files)
        private static string GetEnglishJson()
        {
            return @"{
              ""Greeting"": ""Hello"",
              ""Farewell"": ""Goodbye"",
              ""WelcomeMessage"": ""Welcome to our application!"",
              ""DateFormat"": ""MM/dd/yyyy"",
              ""ThankYou"": ""Thank you for using our service."",
              ""ItemsCount"": ""You have {0} items in your cart.""
            }";
        }

        private static string GetSwedishJson()
        {
            return @"{
              ""Greeting"": ""Hej"",
              ""Farewell"": ""Adjö"",
              ""WelcomeMessage"": ""Välkommen till vår applikation!"",
              ""DateFormat"": ""yyyy-MM-dd"",
              ""ThankYou"": ""Tack för att du använder vår tjänst."",
              ""ItemsCount"": ""Du har {0} objekt i din kundvagn.""
            }";
        }
    }

    public class LocalizationService
    {
        private readonly IStringLocalizer _localizer;
        private readonly ILogger<LocalizationService> _logger;

        public LocalizationService(IStringLocalizer localizer, ILogger<LocalizationService> logger)
        {
            _localizer = localizer;
            _logger = logger;
        }

        public string GetString(string key)
        {
            try
            {
                var localizedString = _localizer[key];
                if (localizedString.ResourceNotFound)
                {
                    _logger.LogWarning($"Resource not found for key: {key}");
                    return $"[{key}]"; // Return key in brackets if not found
                }
                return localizedString.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting localized string for key: {key}");
                return $"[{key}]";
            }
        }

        public string GetString(string key, params object[] arguments)
        {
            try
            {
                var localizedString = _localizer[key, arguments];
                if (localizedString.ResourceNotFound)
                {
                    _logger.LogWarning($"Resource not found for key: {key}");
                    return $"[{key}]";
                }
                return localizedString.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting localized string for key: {key}");
                return $"[{key}]";
            }
        }

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures = true)
        {
            return _localizer.GetAllStrings(includeParentCultures);
        }
    }
}