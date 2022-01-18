using AdoPipelinesLogger.Abstractions;
using AdoPipelinesLogger.Enums;

namespace AdoPipelinesLogger;

public class AdoPipelinesLogger : IAdoPipelinesLogger
{
    private readonly ILogMessageFactory _logMessageFactory;

    public AdoPipelinesLogger(ILogMessageFactory logMessageFactory)
    {
        _logMessageFactory = logMessageFactory;
    }
    
    public void Log(LogFormat logFormat, string message)
    {
        var logMessage = _logMessageFactory.BuildLog(logFormat, message);
        Console.WriteLine(logMessage);
    }

    public void StartLogGroup(string groupName, Action<IAdoPipelinesLogger> logger)
    {
        var startGroupMessage = _logMessageFactory.BuildLog(LogFormat.Group, groupName);
        Console.WriteLine(startGroupMessage);

        logger.Invoke(this);
        
        var endGroupMessage = _logMessageFactory.BuildLog(LogFormat.Endgroup, string.Empty);
        Console.WriteLine(endGroupMessage);

    }
}