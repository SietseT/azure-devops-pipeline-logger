using AdoPipelinesLogger.Enums;

namespace AdoPipelinesLogger.Abstractions;

public interface IAdoPipelinesLogger
{
    void Log(LogFormat logFormat, string message);
    //void LogIssue(LogIssueType issueType, string message, string? sourcePath = null, int? lineNumber = null, int? columnNumber = null, string? code = null);
    void StartLogGroup(string groupName, Action<IAdoPipelinesLogger> logger);
}