using System;
using System.Collections.Generic;
using System.IO;
using AutoFixture.NUnit3;
using AzureDevOps.Logger.Abstractions;
using AzureDevOps.Logger.Enums;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace AzureDevOps.Logger.Tests;

public class AdoPipelinesLoggerTests
{
    private StringWriter _output = null!;
    private IAzDOLogger _azDOLogger = null!;
    private ILogMessageFactory _messageFactory = null!;

    [SetUp]
    public void SetUp()
    {
        _output = new StringWriter();
        Console.SetOut(_output);
        
        _messageFactory = Substitute.For<ILogMessageFactory>();
        _azDOLogger = new AzDOLogger(_messageFactory);
    }

    [TearDown]
    public void TearDown()
    {
        _output.Dispose();
    }
    
    [Test, AutoData]
    public void Log_Once_WritesLineToConsole(string logMessage)
    {
        //Arrange
        _messageFactory.BuildLog(Arg.Any<LogFormat>(), Arg.Is(logMessage)).Returns(logMessage);
        
        // Act
        _azDOLogger.Log(LogFormat.Debug, logMessage);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal(logMessage);
    }
    
    [Test, AutoData]
    public void Log_Twice_WritesLinesToConsole(string logMessage1, string logMessage2)
    {
        //Arrange
        _messageFactory.BuildLog(Arg.Any<LogFormat>(), Arg.Is(logMessage1)).Returns(logMessage1);
        _messageFactory.BuildLog(Arg.Any<LogFormat>(), Arg.Is(logMessage2)).Returns(logMessage2);
        
        // Act
        _azDOLogger.Log(LogFormat.Debug, logMessage1);
        _azDOLogger.Log(LogFormat.Debug, logMessage2);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal(logMessage1, logMessage2);
    }
    
    [Test, AutoData]
    public void LogGroup_WithLog_WritesLinesToConsole(string groupName, string logMessage)
    {
        //Arrange
        _messageFactory.BuildLog(Arg.Is(LogFormat.Group), Arg.Any<string>()).Returns("StartGroup");
        _messageFactory.BuildLog(Arg.Is(LogFormat.Debug), Arg.Is(logMessage)).Returns(logMessage);
        _messageFactory.BuildLog(Arg.Is(LogFormat.Endgroup), Arg.Any<string>()).Returns("EndGroup");
        
        // Act
        _azDOLogger.StartLogGroup(groupName, logger =>
        {
            logger.Log(LogFormat.Debug, logMessage);
        });
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal("StartGroup", logMessage, "EndGroup");
    }
    
    [Test, AutoData]
    public void LogIssue_WithWarning_WritesLinesToConsole(string message, string sourcePath, int lineNumber, int columnNumber, string code)
    {
        //Arrange
        _messageFactory.BuildIssueLog(Arg.Is(LogIssueType.Warning), Arg.Any<string>(), Arg.Any<string?>(), 
            Arg.Any<int>(), Arg.Any<int>(), Arg.Any<string?>()).Returns(message);
        
        // Act
        _azDOLogger.LogIssue(LogIssueType.Warning, message, sourcePath, lineNumber, columnNumber, code);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal(message);
    }
    
    [Test, AutoData]
    public void LogCommand_WithParameters_WritesLinesToConsole(string command, string value)
    {
        // Arrange
        var parameters = new Dictionary<string, string>
        {
            {"parameter1", "value1"},
            {"parameter2", "value2"}
        };

        var expectedValue = $"{command}-{value}";

        _messageFactory.BuildCommandLog(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>?>())
            .Returns(expectedValue);
        
        // Act
        _azDOLogger.LogCommand(command, value, parameters);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal(expectedValue);
    }
    
    [Test, AutoData]
    public void LogProgress_WithValue_WritesLinesToConsole(string message, int value)
    {
        // Arrange
        _messageFactory.BuildCommandLog(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, string>?>())
            .Returns("Progress100");

        // Act
        _azDOLogger.LogProgress(message, value);
        
        // Assert
        _messageFactory.Received().BuildCommandLog(Arg.Is("task.setprogress"), Arg.Is(message),
            Arg.Is<Dictionary<string, string>?>(d => d!["value"] == value.ToString()));
        
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal("Progress100");
    }
}