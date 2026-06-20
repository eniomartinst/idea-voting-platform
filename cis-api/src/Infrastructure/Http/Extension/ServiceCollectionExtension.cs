using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Infrastructure.Http.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CisApi.Src.Infrastructure.Http.Extension;

/// <summary>
/// Registers HTTP-based services used for external integrations.
/// </summary>
public static class ServiceCollectionExtension
{
    /// <summary>
    /// Adds HTTP clients and related services for external APIs.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddHttpServices(this IServiceCollection services)
    {
        services.AddHttpClient<IUserQueryService, UserQueryApiClient>(client =>
        {
            // Base address of the Users API within the Docker network
            client.BaseAddress = new Uri("http://users-api:8001");
        });

        return services;
    }
}