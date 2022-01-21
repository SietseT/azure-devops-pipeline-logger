using AzureDevOps.Logger.Enums;

namespace AzureDevOps.Logger.Abstractions 
{
    public interface IAzDOLogger
    {
        /// <summary>
        /// Logs a formatted command to the pipeline log.
        /// See https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands#logging-command-format for details.
        /// </summary>
        /// <param name="logFormat">Severity/type</param>
        /// <param name="message">Message to log</param>
        void Log(LogFormat logFormat, string message);

        /// <summary>
        /// Logs an issue command to the pipeline log. These show up in your pipeline as a warning or exception, but don't block the pipeline.
        /// See https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands#logissue-log-an-error-or-warning for details.
        /// </summary>
        /// <param name="issueType">Warning or error</param>
        /// <param name="message">Message to log</param>
        /// <param name="sourcePath">Source file location</param>
        /// <param name="lineNumber">Line number</param>
        /// <param name="columnNumber">Column number</param>
        /// <param name="code">Error or warning code</param>
        void LogIssue(LogIssueType issueType, string message, string? sourcePath = null, int? lineNumber = null,
            int? columnNumber = null, string? code = null);

        /// <summary>
        /// Log any type of command to the pipeline log.
        /// See https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands for details.
        /// </summary>
        /// <param name="command">Command name. For example: task.setvariable</param>
        /// <param name="value">Command value</param>
        /// <param name="parameters">Command parameters. Each entry will be passed as a parameter to the command. Example: key1=value1;key2=value2;</param>
        void LogCommand(string command, string value, Dictionary<string, string>? parameters = null);
        
        /// <summary>
        /// Logs a progress command to the pipeline log to inform the progress of the current task.
        /// See https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands#setprogress-show-percentage-completed for details.
        /// </summary>
        /// <param name="message">Progress message</param>
        /// <param name="progress">Percentage between 0 and 100</param>
        void LogProgress(string message, int progress);
        
        /// <summary>
        /// Logs a command group to the pipeline log.
        /// See https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands#formatting-commands for details.
        /// </summary>
        /// <param name="groupName">Name of the group</param>
        /// <param name="logger">Logger object used to log commands that will be shown as part of the group.</param>
        void StartLogGroup(string groupName, Action<IAzDOLogger> logger);
    }
}