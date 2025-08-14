using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Func.Durable.OrderProcessor
{
    public class OrderActivities
    {
        private readonly ILogger<OrderActivities> _logger;

        public OrderActivities(ILogger<OrderActivities> logger)
        {
            _logger = logger;
        }

        [Function("ProcessPayment")]
        public string ProcessPayment([ActivityTrigger] string orderId)
        {
            _logger.LogInformation($"Processing payment for Order {orderId}...");
            // Simulate payment API call
            return "Success";
        }

        [Function("UpdateInventory")]
        public string UpdateInventory([ActivityTrigger] string orderId)
        {
            _logger.LogInformation($"Updating inventory for Order {orderId}...");
            // Simulate inventory system update
            return "Success";
        }

        [Function("ArrangeShipping")]
        public string ArrangeShipping([ActivityTrigger] string orderId)
        {
            _logger.LogInformation($"Arranging shipping for Order {orderId}...");
            // Simulate shipping API call
            return "Success";
        }

        [Function("SendConfirmation")]
        public void SendConfirmation([ActivityTrigger] string orderId)
        {
            _logger.LogInformation($"Sending confirmation for Order {orderId}...");
            // Simulate email or SMS notification
        }
    }
}
