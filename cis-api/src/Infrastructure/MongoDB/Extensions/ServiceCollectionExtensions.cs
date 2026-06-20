using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Infrastructure.MongoDB.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace CisApi.Src.Infrastructure.MongoDB.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMongoInfrastructureServices(
        this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MongoDB");
        var client = new MongoClient(connectionString);
        var database = client.GetDatabase("sd3_cis_db");

        services.AddSingleton<IMongoDatabase>(database);

        services.AddScoped<ITopicRepository, TopicRepository>();
        services.AddScoped<IIdeaRepository, IdeaRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();

        return services;
    }
}
