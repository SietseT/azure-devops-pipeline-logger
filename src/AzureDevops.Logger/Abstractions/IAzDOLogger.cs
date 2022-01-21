using AzureDevOps.Logger.Enums;

namespace AzureDevOps.Logger.Abstractions 
{
    public interface IAzDOLogger
    {
        void Log(LogFormat logFormat, string message);

        void LogIssue(LogIssueType issueType, string message, string? sourcePath = null, int? lineNumber = null,
            int? columnNumber = null, string? code = null);

        void LogCommand(string command, string value, Dictionary<string, string>? parameters = null);
        void LogProgress(string message, int progress);
        void StartLogGroup(string groupName, Action<IAzDOLogger> logger);
    }
}