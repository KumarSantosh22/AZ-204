using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Func.HttpTriggered
{
    public class FuncTwo
    {
        private readonly ILogger<FuncTwo> _logger;

        public FuncTwo(ILogger<FuncTwo> logger)
        {
            _logger = logger;
        }

        [Function("functwo")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = "functwo/{id}")] HttpRequest req, string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            _logger.LogInformation("Processing request...");

            // Get query string value
            string queryValue = req.Query["name"];

            // Get route value (already in `id`)
            string routeValue = id;

            // Get body value
            string bodyValue = string.Empty;
            using(var reader = new StreamReader(req.Body))
    {
                var body = await reader.ReadToEndAsync();
                if (!string.IsNullOrWhiteSpace(body))
                {
                    var data = JsonSerializer.Deserialize<Dictionary<string, string>>(body);
                    data?.TryGetValue("name", out bodyValue);
                }
            }

            return new OkObjectResult(new
            {
                FromQuery = queryValue,
                FromRoute = routeValue,
                FromBody = bodyValue
            });
        }
    }
}
