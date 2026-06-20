using CisApi.Src.Core.Application.Services;
using CisApi.Src.Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CisApi.Src.Core.Application.Extensions;

/// <summary>
/// Centralizes dependency injection configuration for the Application layer.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers application layer services (business logic).
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // --- Command Services (Writing) ---
        services.AddScoped<ITopicCommandService, TopicCommandService>();
        services.AddScoped<IIdeaCommandService, IdeaCommandService>();

        // --- Query Services (Reading) ---
        services.AddScoped<ITopicQueryService, TopicQueryService>();
        services.AddScoped<IIdeaQueryService, IdeaQueryService>();
        services.AddScoped<IVoteQueryService, VoteQueryService>();

        return services;
    }
}