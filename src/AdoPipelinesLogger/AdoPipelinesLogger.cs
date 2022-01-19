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

    public void LogIssue(LogIssueType issueType, string message, string? sourcePath = null, int? lineNumber = null,
        int? columnNumber = null, string? code = null)
    {
        var logMessage = _logMessageFactory.BuildIssueLog(issueType, message, sourcePath, lineNumber, columnNumber, code);
        Console.WriteLine(logMessage);
    }

    public void LogCommand(string command, string value, Dictionary<string, string>? parameters = null)
    {
        var logMessage = _logMessageFactory.BuildCommandLog(command, value, parameters);
        Console.WriteLine(logMessage);
    }

    public void LogProgress(string message, int progress)
    {
        var logMessage = _logMessageFactory.BuildCommandLog("task.setprogress", message, new Dictionary<string, string>
        {
            { "value", progress.ToString()}
        });
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