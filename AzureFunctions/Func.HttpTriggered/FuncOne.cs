using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Func.HttpTriggered
{
    public class FuncOne
    {
        private readonly ILogger<FuncOne> _logger;

        public FuncOne(ILogger<FuncOne> logger)
        {
            _logger = logger;
        }

        [Function("funcone")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
