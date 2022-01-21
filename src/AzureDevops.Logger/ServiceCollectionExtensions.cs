using AzureDevOps.Logger.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace AzureDevOps.Logger;

public static class ServiceCollectionExtensions
{
    public static void AddAzureDevOpsLogger(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<ILogMessageFactory, LogMessageFactory>();
        serviceCollection.AddSingleton<IAzDOLogger, AzDOLogger>();
    }
}