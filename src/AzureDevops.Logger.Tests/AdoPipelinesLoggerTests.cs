using System;
using System.Collections.Generic;
using System.IO;
using AutoFixture.NUnit3;
using AzureDevOps.Logger.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace AzureDevOps.Logger.Tests;

public class AdoPipelinesLoggerTests
{
    private StringWriter _output = null!;

    [SetUp]
    public void SetUp()
    {
        _output = new StringWriter();
        Console.SetOut(_output);
    }

    [TearDown]
    public void TearDown()
    {
        _output.Dispose();
    }
    
    [Test, AutoData]
    public void LogOnce_WritesLineToConsole(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        var logger = new AzDOLogger(factory);
        
        // Act
        logger.Log(LogFormat.Debug, logMessage);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal($"##[debug]{logMessage}");
    }
    
    [Test, AutoData]
    public void LogTwice_WritesLinesToConsole(string logMessage1, string logMessage2)
    {
        // Arrange
        var factory = new LogMessageFactory();
        var logger = new AzDOLogger(factory);
        
        // Act
        logger.Log(LogFormat.Debug, logMessage1);
        logger.Log(LogFormat.Debug, logMessage2);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal($"##[debug]{logMessage1}", $"##[debug]{logMessage2}");
    }
    
    [Test, AutoData]
    public void LogGroup_WritesLinesToConsole(string groupName, string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        var sut = new AzDOLogger(factory);
        
        // Act
        sut.StartLogGroup(groupName, logger =>
        {
            logger.Log(LogFormat.Debug, logMessage);
        });
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal($"##[group]{groupName}", $"##[debug]{logMessage}", "##[endgroup]");
    }
    
    [Test, AutoData]
    public void LogIssue_WritesLinesToConsole(string message, string sourcePath, int lineNumber, int columnNumber, string code)
    {
        // Arrange
        var factory = new LogMessageFactory();
        var sut = new AzDOLogger(factory);
        
        // Act
        sut.LogIssue(LogIssueType.Warning, message, sourcePath, lineNumber, columnNumber, code);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal($"##vso[task.logissue type=warning;sourcepath={sourcePath};linenumber={lineNumber};columnnumber={columnNumber};code={code};]{message}");
    }
    
    [Test, AutoData]
    public void LogCommand_WithParameters_WritesLinesToConsole(string command, string value)
    {
        // Arrange
        var factory = new LogMessageFactory();
        var parameters = new Dictionary<string, string>
        {
            {"parameter1", "value1"},
            {"parameter2", "value2"}
        };
        
        var sut = new AzDOLogger(factory);
        
        // Act
        sut.LogCommand(command, value, parameters);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal($"##vso[{command} parameter1=value1;parameter2=value2;]{value}");
    }
    
    [Test, AutoData]
    public void LogProgress_WritesLinesToConsole(string message, int value)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        var sut = new AzDOLogger(factory);
        
        // Act
        sut.LogProgress(message, value);
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal($"##vso[task.setprogress value={value};]{message}");
    }
}