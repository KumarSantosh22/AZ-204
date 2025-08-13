using Azure.Messaging.EventHubs;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Func.EventHubTriggered;
public class FuncOne
{
    private readonly ILogger<FuncOne> _logger;

    public FuncOne(ILogger<FuncOne> logger)
    {
        _logger = logger;
    }

    [Function(nameof(FuncOne))]
    public void Run([EventHubTrigger(eventHubName:"samples-workitems", Connection= "EvhConnString")] EventData[] events)
    {
        foreach (EventData @event in events)
        {
            _logger.LogInformation("Event Body: {body}", @event.Body);
            _logger.LogInformation("Event Content-Type: {contentType}", @event.ContentType);
        }
    }
}