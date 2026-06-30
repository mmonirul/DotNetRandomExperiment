using Microsoft.Extensions.Localization;
using System.Globalization;

namespace LocalizerWorkerService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Services.AddLocalization(options =>
        {
            options.ResourcesPath = "Resources";
        });

        builder.Services.AddSingleton<IStringLocalizerFactory>(provider => new JsonStringLocalizerFactory("Resources"));

        builder.Services.AddTransient(typeof(IStringLocalizer<>), typeof(StringLocalizer<>));

        builder.Services.AddHostedService<Worker>();

        var host = builder.Build();

        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en");

        host.Run();
    }
}