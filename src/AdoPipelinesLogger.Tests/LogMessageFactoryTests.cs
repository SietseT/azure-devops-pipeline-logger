using AdoPipelinesLogger.Enums;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace AdoPipelinesLogger.Tests;

public class LogMessageFactoryTests
{

    [Test, AutoData]
    public void Warning_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Warning, logMessage);
        
        // Assert
        sut.Should().Be($"##[warning]{logMessage}");
    }
    
    [Test, AutoData]
    public void Error_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Error, logMessage);
        
        // Assert
        sut.Should().Be($"##[error]{logMessage}");
    }
    
    [Test, AutoData]
    public void Section_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Section, logMessage);
        
        // Assert
        sut.Should().Be($"##[section]{logMessage}");
    }
    
    [Test, AutoData]
    public void Debug_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Debug, logMessage);
        
        // Assert
        sut.Should().Be($"##[debug]{logMessage}");
    }
    
    [Test, AutoData]
    public void Command_ReturnsCorrectMessage(string logMessage)
    {
        // Arrange
        var factory = new LogMessageFactory();
        
        // Act
        var sut = factory.BuildLog(LogFormat.Command, logMessage);
        
        // Assert
        sut.Should().Be($"##[command]{logMessage}");
    }
}