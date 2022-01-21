using System.Collections.Generic;
using AutoFixture.NUnit3;
using AzureDevOps.Logger.Enums;
using FluentAssertions;
using NUnit.Framework;

namespace AzureDevOps.Logger.Tests;

public class LogMessageFactoryTests
{

    [Test, AutoData]
    public void BuildLog_ForWarning_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Warning, logMessage);
        
        // Assert
        sut.Should().Be($"##[warning]{logMessage}");
    }
    
    [Test, AutoData]
    public void BuildLog_ForError_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Error, logMessage);
        
        // Assert
        sut.Should().Be($"##[error]{logMessage}");
    }
    
    [Test, AutoData]
    public void BuildLog_ForSection_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Section, logMessage);
        
        // Assert
        sut.Should().Be($"##[section]{logMessage}");
    }
    
    [Test, AutoData]
    public void BuildLog_ForGroup_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Group, logMessage);
        
        // Assert
        sut.Should().Be($"##[group]{logMessage}");
    }
    
    [Test, AutoData]
    public void BuildLog_ForEndGroup_ReturnsCorrectMessage()
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Endgroup, string.Empty);
        
        // Assert
        sut.Should().Be($"##[endgroup]");
    }
    
    [Test, AutoData]
    public void BuildLog_ForDebug_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Debug, logMessage);
        
        // Assert
        sut.Should().Be($"##[debug]{logMessage}");
    }
    
    [Test, AutoData]
    public void BuildLog_ForCommand_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Command, logMessage);
        
        // Assert
        sut.Should().Be($"##[command]{logMessage}");
    }
    
    [Test, AutoData]
    public void BuildCommandLog_ReturnsCorrectMessage(string command, string value)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildCommandLog(command, value);
        
        // Assert
        sut.Should().Be($"##vso[{command}]{value}");
    }
    
    [Test, AutoData]
    public void BuildCommandLog_WithParameters_ReturnsCorrectMessage(string command, string value)
    {
        // Arrange
        var factory = new LogMessageFactory();
        var parameters = new Dictionary<string, string>
        {
            {"parameter1", "value1"},
            {"parameter2", "value2"}
        };
        
        // Act
        var sut = factory.BuildCommandLog(command, value, parameters);
        
        // Assert
        sut.Should().Be($"##vso[{command} parameter1=value1;parameter2=value2;]{value}");
    }
    
    [Test, AutoData]
    public void BuildIssueLog_Warning_ReturnsCorrectMessage(string message)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildIssueLog(LogIssueType.Warning, message);
        
        // Assert
        sut.Should().Be($"##vso[task.logissue type=warning]{message}");
    }
    
    [Test, AutoData]
    public void BuildIssueLog_WithAllParameters_ReturnsCorrectMessage(string message, string sourcePath, int lineNumber, int columnNumber, string code)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildIssueLog(LogIssueType.Error, message, sourcePath, lineNumber, columnNumber, code);
        
        // Assert
        sut.Should().Be($"##vso[task.logissue type=error;sourcepath={sourcePath};linenumber={lineNumber};columnnumber={columnNumber};code={code};]{message}");
    }
    
    [Test, AutoData]
    public void BuildIssueLog_WithSomeParameters_ReturnsCorrectMessage(string message, string sourcePath, string code)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildIssueLog(LogIssueType.Error, message, sourcePath, code: code);
        
        // Assert
        sut.Should().Be($"##vso[task.logissue type=error;sourcepath={sourcePath};code={code};]{message}");
    }
}