using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Func.Durable.OrderProcessor
{
    public class OrderStarter
    {
        private readonly ILogger<OrderStarter> _logger;

        public OrderStarter(ILogger<OrderStarter> logger)
        {
            _logger = logger;
        }

        [Function("OrderStarter")]
        public async Task<HttpResponseData> Start(
            [HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequestData req,
            [DurableClient] DurableTaskClient client)
        {
            string orderId = Guid.NewGuid().ToString();

            _logger.LogInformation($"Starting order workflow for Order {orderId}...");

            // Start orchestration
            var instanceId = await client.ScheduleNewOrchestrationInstanceAsync(
                "OrderOrchestrator", orderId);

            var response = req.CreateResponse(System.Net.HttpStatusCode.Accepted);
            await response.WriteStringAsync($"Order process started. Instance ID: {instanceId}");
            return response;
        }
    }

}
