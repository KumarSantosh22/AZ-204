using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Func.TimerTriggered;

public class FunctionOne
{
    private readonly ILogger _logger;

    public FunctionOne(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<FunctionOne>();
    }

    [Function("FuncOne")]
    public void Run([TimerTrigger("0 */5 * * * *", RunOnStartup =true)] TimerInfo myTimer)
    {
        _logger.LogInformation("C# Timer trigger function executed at: {executionTime}", DateTime.Now);
        
        if (myTimer.ScheduleStatus is not null)
        {
            _logger.LogInformation("Hello! Next timer schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
        }
    }
}