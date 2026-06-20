using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Infrastructure.MySQL.Context;
using CisApi.Src.Infrastructure.MySQL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Test.Infrastructure.MySQL.Repositories;

public class IdeaRepositoryTests
{
    private DbContextOptions<MyDbContext> _options = new DbContextOptionsBuilder<MyDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    [Fact]
    public async Task CreateAndGetIdea_ShouldWorkCorrectly()
    {
        using var context = new MyDbContext(_options);
        var repo = new IdeaRepository(context);

        var idea = new Idea
        {
            Id = "I1",
            TopicId = "T1",
            UserId = "U1",
            Content = "watch movies",
            VotesCount = 0
        };

        await repo.CreateIdeaAsync(idea);

        var byId = await repo.GetIdeaByIdAsync("I1");
        var byTopic = await repo.GetIdeasByTopicIdAsync("T1");

        Assert.NotNull(byId);
        Assert.Single(byTopic);
        Assert.Equal("watch movies", byId!.Content);
    }

    [Fact]
    public async Task GetIdeaById_ShouldReturnNull_WhenNotFound()
    {
        var emptyOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "EmptyDb_" + Guid.NewGuid().ToString())
            .Options;

        using var context = new MyDbContext(emptyOptions);
        var repo = new IdeaRepository(context);

        var result = await repo.GetIdeaByIdAsync("INVALID");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetIdeasByTopicId_ShouldReturnEmpty_WhenNoIdeasExist()
    {
        using var context = new MyDbContext(_options);
        var repo = new IdeaRepository(context);

        var result = await repo.GetIdeasByTopicIdAsync("NO_TOPIC");

        Assert.Empty(result);
    }
}