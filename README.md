# Azure DevOps Pipeline Logger
[![CodeQL and tests](https://github.com/SietseT/azure-devops-pipeline-logger/actions/workflows/codeql-analysis.yml/badge.svg)](https://github.com/SietseT/azure-devops-pipeline-logger/actions/workflows/codeql-analysis.yml) ![Nuget](https://img.shields.io/nuget/v/AzureDevOps.Logger)


Simple .NET package to log specific Azure DevOps commands during a pipeline run. A use-case could be when you're developing a custom console application (e.g. a CLI tool) that is used in Azure DevOps pipelines. This package enables you to send [logging commands](https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash) to the pipeline in a simple and easy way.

The package exposes a logger class where called methods will be converted to the appropiate log commands defined by Microsoft. For example:
```csharp
logger.LogCommand("task.setvariable", "secretvalue",  new Dictionary<string, string>
{
    {"variable", "secret"},
    {"issecret", "true"}
});
```
results in the following command being logged to the pipeline:
```
##vso[task.setvariable variable=secret;issecret=true;]secretvalue
```

A complete reference of logging commands can be found in the [Microsoft Docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash).

## Features
- Log messages with supported severity levels
- Create log groups
- Log commands (examples: set variable, upload artifact)
- Set task progress

## Installation

Install the NuGet package [AzureDevOps.Logger](https://www.nuget.org/packages/AzureDevOps.Logger/0.1.0-azure-pipelines0035). Then follow one of the registration methods below.

### Method 1: Dependency injection (preferred)
Register the required services, mostly in the `ConfigureServices` of the `Startup` class, using the provived extension method:
```csharp
services.AddAzureDevOpsLogger();
```

If you're using a custom DI container, make sure to register the following services and implementation:

```
container.AddSingleton<ILogMessageFactory, LogMessageFactory>();
container.AddSingleton<IAzDOLogger, AzDOLogger>();
```

After registration, the `IAzDOLogger` service is ready to be injected wherever you need them, for example:
```csharp
public WeatherForecastController(IAzDOLogger logger)
{
    _logger = logger;
}
```

### Method 2: Manual
You can manually create an instance of the logger with the following snippet:

```csharp
var logger = new AzDOLogger(new LogMessageFactory());
```

## Usage

The section below shows how to use the NuGet package. Please refer to the Microsoft Docs to see how the log commands will show up in your pipeline and what they do. 

### Log formatting commands ([docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash#formatting-commands))

The `Log()` method can be used to log simple formatting commands to the pipeline log with a specific severity. These logs **don't** show up in the pipeline results.

```csharp
logger.Log(LogFormat.Section, "This is a section message");
logger.Log(LogFormat.Command, "This is a command message");
logger.Log(LogFormat.Debug, "This is a debug message");
logger.Log(LogFormat.Error, "This is a error message");
logger.Log(LogFormat.Warning, "This is a warning message");
```

### Log group of formatting commands

The `StartLogGroup()` method can be used indicate the start of a group of formatted log commands. A group can be collapsed in the pipeline log.

```csharp
logger.StartLogGroup("This is a group", pipelinesLogger =>
{
    pipelinesLogger.Log(LogFormat.Section, "This is a section message");
    pipelinesLogger.Log(LogFormat.Command, "This is a command message");
    pipelinesLogger.Log(LogFormat.Debug, "This is a debug message");
    pipelinesLogger.Log(LogFormat.Error, "This is a error message");
    pipelinesLogger.Log(LogFormat.Warning, "This is a warning message");
});
```

### Log issues ([docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash#logissue-log-an-error-or-warning))
The `LogIssue()` method can be used to log warnings and errors to the pipeline. These **will** show up in the pipeline results as either warning or error. Keep in mind that this does not automatically stop a pipeline from executing like an actual error would.

### Log progress command ([docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash#setprogress-show-percentage-completed))
The `LogProgress` method can be used to log a progress command to the pipeline log. The progress percentage will be shown instead of the default 'task running' spinner.
```csharp
logger.LogProgress("Log running taks", 10);
```

### Log commands ([docs](https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash#task-commands))
For other commands, like artifact commands, you can use the `LogCommand()` method. Using this method you can log all the commands that are specified in the docs.

For example, to [set a variable](https://docs.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands?view=azure-devops&tabs=bash#setvariable-initialize-or-modify-the-value-of-a-variable), the following code can be used:
```csharp
logger.LogCommand("task.setvariable", "secretvalue",  new Dictionary<string, string>
{
    {"variable", "secret"},
    {"issecret", "true"}
});
```

The result is the following log command:
```
##vso[task.setvariable variable=secret;issecret=true;]secretvalue
```

## Contributing
See [Contributing guidelines](CONTRIBUTING.md).
