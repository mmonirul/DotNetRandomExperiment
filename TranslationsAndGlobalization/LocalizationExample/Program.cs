using LocalizationExample.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Globalization;

namespace LocalizationExample;

class Program
{
    static void Main(string[] args)
    {
        // Create host builder
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Add localization services
                services.AddLocalization(options => options.ResourcesPath = "");
                services.AddScoped<LocalizationService>();
            })
            .Build();

        // Get the localization service
        var localizationService = host.Services.GetRequiredService<LocalizationService>();

        Console.WriteLine("=== .NET 8 Localization Example ===");
        Console.WriteLine("Supported languages:");
        Console.WriteLine("- en (English) - Default");
        Console.WriteLine("- sv (Swedish)");
        Console.WriteLine("- es (Spanish)");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Enter language code (en/sv/es) or 'exit' to quit: ");
            var input = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(input) || input == "exit")
            {
                Console.WriteLine("Goodbye!");
                break;
            }

            try
            {
                switch (input)
                {
                    case "en":
                        localizationService.SetCulture("en-US");
                        Console.WriteLine("\n--- English (Default) ---");
                        break;
                    case "sv":
                        localizationService.SetCulture("sv-SE");
                        Console.WriteLine("\n--- Swedish ---");
                        break;
                    case "es":
                        localizationService.SetCulture("es-ES");
                        Console.WriteLine("\n--- Spanish ---");
                        break;
                    default:
                        Console.WriteLine("Invalid language code. Please enter 'en', 'sv', or 'es'.");
                        continue;
                }

                // Display localized greetings
                localizationService.DisplayGreetings();

                // Additional demonstration using resource manager directly
                Console.WriteLine("--- Additional demonstration using ResourceManager ---");
                var resourceManager = LocalizationExample.Resources.Messages.ResourceManager;

                // Set culture for resource manager
                LocalizationExample.Resources.Messages.Culture = CultureInfo.CurrentUICulture;

                Console.WriteLine($"Welcome: {LocalizationExample.Resources.Messages.Welcome}");
                Console.WriteLine($"Current Time: {string.Format(LocalizationExample.Resources.Messages.CurrentTime, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))}");
                Console.WriteLine($"Goodbye: {LocalizationExample.Resources.Messages.Goodbye}");

                Console.WriteLine($"Category: {LocalizationExample.Resources.Category.Category.Food}");

                Console.WriteLine();
            }
            catch (CultureNotFoundException)
            {
                Console.WriteLine($"Culture '{input}' is not supported. Using default culture (English).");
                localizationService.SetCulture("en-US");
                localizationService.DisplayGreetings();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
