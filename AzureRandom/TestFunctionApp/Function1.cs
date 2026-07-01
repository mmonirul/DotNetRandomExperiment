using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace TestFunctionApp
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;

        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }

        [Function("Function1")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }

        [Function("GetNumber")]
        public IActionResult GetNumber([HttpTrigger(AuthorizationLevel.Function, "get", Route = "getnumber/{number:int}")] HttpRequest req, int number)
        {
            _logger.LogInformation($"Received number: {number}");
            return new OkObjectResult($"You sent the number: {number}");
        }
    }
}
