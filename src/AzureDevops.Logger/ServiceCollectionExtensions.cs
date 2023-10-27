using AzureDevOps.Logger.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AzureDevOps.Logger;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers the required services, so that <see cref="IAzDOLogger"/> can be injected and used in the application.
    /// </summary>
    /// <param name="serviceCollection"><see cref="IServiceCollection"/> instance.</param>
    public static IServiceCollection AddAzureDevOpsLogger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILogMessageFactory, LogMessageFactory>();
        serviceCollection.AddSingleton<IAzDOLogger, AzDOLogger>();
        return serviceCollection;
    }
}