using CisApi.Src.Core.Application.Extensions;
using CisApi.Src.Infrastructure.Auth.Extension;
using CisApi.Src.Infrastructure.Http.Extension;
using CisApi.Src.Presentation.RestApi.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace CisApi.Src.Infrastructure.IoC;

/// <summary>
/// Centralizes dependency injection configuration for the application.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers all application layers (Application, Infrastructure, Presentation).
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddCisServices(this IServiceCollection services)
    {
        // Register application layer services (business logic)
        services.AddApplicationServices();

        // Register infrastructure layer services (integrations)
        services.AddAuthServices();
        services.AddHttpServices();

        // Register presentation layer services (controllers, validation)
        services.AddPresentationServices();

        return services;
    }
}