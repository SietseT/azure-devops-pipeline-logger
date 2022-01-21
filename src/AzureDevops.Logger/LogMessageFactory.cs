using System.Text;
using AzureDevOps.Logger.Abstractions;
using AzureDevOps.Logger.Enums;

namespace AzureDevOps.Logger
{
    public class LogMessageFactory : ILogMessageFactory
    {
        public string BuildLog(LogFormat logFormat, string message)
        {
            var logType = GetLogFormatType(logFormat);
            return $"##[{logType}]{message}";
        }

        public string BuildCommandLog(string command, string value, Dictionary<string, string>? parameters = null)
        {
            if (parameters == null || !parameters.Any()) return $"##vso[{command}]{value}";

            var parameterStrings = parameters.Select(p => $"{p.Key}={p.Value}");
            var parameterString = string.Join(";", parameterStrings);
            return $"##vso[{command} {parameterString};]{value}";
        }

        public string BuildIssueLog(LogIssueType issueType, string message, string? sourcePath = null,
            int? lineNumber = null,
            int? columnNumber = null, string? code = null)
        {
            var issueTypeString = GetLogIssueType(issueType);

            var optionalArguments = new List<string>();
            if (!string.IsNullOrWhiteSpace(sourcePath))
                optionalArguments.Add($"sourcepath={sourcePath}");
            if (lineNumber != null)
                optionalArguments.Add($"linenumber={lineNumber}");
            if (lineNumber != null)
                optionalArguments.Add($"columnnumber={columnNumber}");
            if (!string.IsNullOrWhiteSpace(code))
                optionalArguments.Add($"code={code}");

            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"##vso[task.logissue type={issueTypeString}");
            if (optionalArguments.Any())
            {
                stringBuilder.Append(';');
                stringBuilder.Append(string.Join(";", optionalArguments));
                stringBuilder.Append(';');
            }

            stringBuilder.Append($"]{message}");

            return stringBuilder.ToString();
        }

        private static string GetLogFormatType(LogFormat logFormat)
        {
            return logFormat switch
            {
                LogFormat.Warning => "warning",
                LogFormat.Error => "error",
                LogFormat.Section => "section",
                LogFormat.Debug => "debug",
                LogFormat.Command => "command",
                LogFormat.Group => "group",
                LogFormat.Endgroup => "endgroup",
                _ => throw new ArgumentOutOfRangeException(nameof(logFormat), logFormat, null)
            };
        }

        private static string GetLogIssueType(LogIssueType issueType)
        {
            return issueType switch
            {
                LogIssueType.Warning => "warning",
                LogIssueType.Error => "error",
                _ => throw new ArgumentOutOfRangeException(nameof(issueType), issueType, null)
            };
        }
    }
}