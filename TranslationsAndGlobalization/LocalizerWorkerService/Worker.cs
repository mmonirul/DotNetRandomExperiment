using Microsoft.Extensions.Localization;

namespace LocalizerWorkerService;

public class Worker : BackgroundService
{
    private readonly IStringLocalizer<Worker> _localizer;
    private readonly ILogger<Worker> _logger;

    public Worker(IStringLocalizer<Worker> localizer, ILogger<Worker> logger)
    {
        _localizer = localizer;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            var greeting = _localizer["Greeting"];
            var status = _localizer["Processing"];

            _logger.LogInformation(greeting);
            _logger.LogInformation(status);

            //_localizer.GetAllStrings(includeParentCultures: true)
            //    .ToList()
            //    .ForEach(s => _logger.LogInformation($"Localized string: {s.Name} = {s.Value}"));

            var ss = new JsonStringLocalizer("Resources", "resources.json");
            var s = ss.GetAllTranslations("Greeting");

            // Example with parameters
            var progress = _localizer["Progress", 25];
            _logger.LogInformation(progress);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
