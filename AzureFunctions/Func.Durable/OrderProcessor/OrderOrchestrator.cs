using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace Func.Durable.OrderProcessor;

public static class OrderOrchestrator
{
    [Function(nameof(OrderOrchestrator))]
    public static async Task RunOrchestrator(
        [OrchestrationTrigger] TaskOrchestrationContext context)
    {
        ILogger logger = context.CreateReplaySafeLogger(nameof(OrderOrchestrator));
        logger.LogInformation("Started order processing...");

        var orderId = context.GetInput<string>();

        // Step 1: Process payment
        var paymentResult = await context.CallActivityAsync<string>("ProcessPayment", orderId);
        if (paymentResult != "Success")
        {
            context.SetCustomStatus("Payment failed");
            return;
        }

        // Step 2: Update inventory
        var inventoryResult = await context.CallActivityAsync<string>("UpdateInventory", orderId);
        if (inventoryResult != "Success")
        {
            context.SetCustomStatus("Inventory update failed");
            return;
        }

        // Step 3: Arrange shipping
        var shippingResult = await context.CallActivityAsync<string>("ArrangeShipping", orderId);
        if (shippingResult != "Success")
        {
            context.SetCustomStatus("Shipping arrangement failed");
            return;
        }

        // Step 4: Send confirmation
        await context.CallActivityAsync("SendConfirmation", orderId);

        context.SetCustomStatus("Order completed successfully");
    }
}