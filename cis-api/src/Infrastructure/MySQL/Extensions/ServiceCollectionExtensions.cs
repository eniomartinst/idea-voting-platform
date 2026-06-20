using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Infrastructure.MySQL.Context;
using CisApi.Src.Infrastructure.MySQL.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CisApi.Src.Infrastructure.MySQL.Extensions;

/// <summary>
/// Configures infrastructure services such as database and repositories.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers MySQL database context and repository implementations.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configuration">Application configuration.</param>
    /// <returns>The updated service collection.</returns>
    public static IServiceCollection AddMySqlInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        // EF Core MySQL configuration (fixed version + retry policy)
        services.AddDbContext<MyDbContext>(options =>
            options.UseMySql(
                configuration.GetConnectionString("CISDB"),
                new MySqlServerVersion(new Version(8, 0, 0)),
                opt => opt.EnableRetryOnFailure()
            ));

        // Register repositories for entities
        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IIdeaRepository, IdeaRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();

        return services;
    }
}
