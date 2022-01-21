using System;
using AzureDevOps.Logger.Abstractions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace AzureDevOps.Logger.Tests;

public class DependencyInjectionTests
{
    [Test]
    public void ResolveLogger_AfterRegistrationWithExtensionMethod_ReturnsInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddAzureDevOpsLogger();
        
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var sut = serviceProvider.GetRequiredService<IAzDOLogger>();

        // Assert
        sut.Should().NotBeNull();
        sut.Should().BeOfType<AzDOLogger>();
    }
    
    [Test]
    public void ResolveLogger_AfterManualRegistration_ReturnsInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddScoped<ILogMessageFactory, LogMessageFactory>();
        services.AddScoped<IAzDOLogger, AzDOLogger>();
        
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var sut = serviceProvider.GetRequiredService<IAzDOLogger>();

        // Assert
        sut.Should().NotBeNull();
        sut.Should().BeOfType<AzDOLogger>();
    }
    
    [Test]
    public void ResolveLogger_WithoutRegistration_ShouldThrow()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceProvider = services.BuildServiceProvider();

        // Act
        var action = () => serviceProvider.GetRequiredService<IAzDOLogger>();

        // Assert
        action.Should().Throw<InvalidOperationException>();
    }
}