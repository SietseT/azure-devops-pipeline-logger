using AdoPipelinesLogger.Enums;

namespace AdoPipelinesLogger.Abstractions;

public interface ILogMessageFactory
{
    string BuildLog(LogFormat logFormat, string message);
    string BuildIssueLog(LogIssueType issueType, string message, string? sourcePath = null, int? lineNumber = null, int? columnNumber = null, string? code = null);
}