using AdoPipelines.Logger.Enums;

namespace AdoPipelines.Logger.Abstractions
{
    public interface ILogMessageFactory
    {
        string BuildLog(LogFormat logFormat, string message);
        string BuildCommandLog(string command, string value, Dictionary<string, string>? parameters = null);
        string BuildIssueLog(LogIssueType issueType, string message, string? sourcePath = null, int? lineNumber = null, int? columnNumber = null, string? code = null);
    }
}

