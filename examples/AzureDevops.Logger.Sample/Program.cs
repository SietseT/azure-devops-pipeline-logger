﻿using AzureDevOps.Logger;
using AzureDevOps.Logger.Enums;
using AzureDevOps.Logger.Sample;
using CommandLine;

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(RunOptions);

static void RunOptions(Options options)
{
    switch (options.Type)
    {
        case "log":
            Log();
            break;
        case "group":
            LogGroup();
            break;
        case "issue":
            LogIssue();
            break;
        case "command":
            LogCommand();
            break;
        case "progress":
            LogProgress();
            break;
    }
}

static void Log()
{
    var logger = new AzDOLogger(new LogMessageFactory());
    logger.Log(LogFormat.Section, "This is a section message");
    logger.Log(LogFormat.Command, "This is a command message");
    logger.Log(LogFormat.Debug, "This is a debug message");
    logger.Log(LogFormat.Error, "This is a error message");
    logger.Log(LogFormat.Warning, "This is a warning message");
    
}

static void LogGroup()
{
    var logger = new AzDOLogger(new LogMessageFactory());
    logger.StartLogGroup("This is a group", pipelinesLogger =>
    {
        pipelinesLogger.Log(LogFormat.Section, "This is a section message");
        pipelinesLogger.Log(LogFormat.Command, "This is a command message");
        pipelinesLogger.Log(LogFormat.Debug, "This is a debug message");
        pipelinesLogger.Log(LogFormat.Error, "This is a error message");
        pipelinesLogger.Log(LogFormat.Warning, "This is a warning message");
    });
}

static void LogIssue()
{
    var logger = new AzDOLogger(new LogMessageFactory());
    logger.Log(LogFormat.Section, "The two loglines below will show up in the build log of the pipeline.");
    logger.LogIssue(LogIssueType.Warning, "This is a warning message");
    logger.LogIssue(LogIssueType.Error, "This is a error message");
}

static void LogCommand()
{
    var logger = new AzDOLogger(new LogMessageFactory());
    logger.LogCommand("task.setvariable", "secretvalue",  new Dictionary<string, string>
    {
        {"variable", "secret"},
        {"issecret", "true"}
    });
}

static void LogProgress()
{
    var logger = new AzDOLogger(new LogMessageFactory());
    for (var i = 0; i <= 100; i += 10)
    {
        Thread.Sleep(3000);
        logger.LogProgress("Current progress", i);
    }
}