using System;
using System.IO;
using AdoPipelinesLogger.Enums;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace AdoPipelinesLogger.Tests;

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
        var logger = new AdoPipelinesLogger(factory);
        
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
        var logger = new AdoPipelinesLogger(factory);
        
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
        var sut = new AdoPipelinesLogger(factory);
        
        // Act
        sut.StartLogGroup(groupName, logger =>
        {
            logger.Log(LogFormat.Debug, logMessage);
        });
        
        // Assert
        var stringOutput = _output.ToString().Split( Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        stringOutput.Should().Equal($"##[group]{groupName}", $"##[debug]{logMessage}", "##[endgroup]");
    }
}