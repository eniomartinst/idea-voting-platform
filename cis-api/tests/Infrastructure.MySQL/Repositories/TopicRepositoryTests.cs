using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Infrastructure.MySQL.Context;
using CisApi.Src.Infrastructure.MySQL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Test.Infrastructure.MySQL.Repositories;

public class TopicRepositoryTests
{
    private DbContextOptions<MyDbContext> _options = new DbContextOptionsBuilder<MyDbContext>()
        .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
        .Options;

    [Fact]
    public async Task CreateAndGetTopic_ShouldWorkCorrectly()
    {
        using var context = new MyDbContext(_options);
        var repo = new TopicRepository(context);
        var topic = new Topic { Id = "T1", Title = "Test", UserId = "U1" };

        await repo.CreateTopicAsync(topic);
        await repo.GetTopicAsync("T1");
        await repo.GetTopicsAsync();

        Assert.True(true);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldPersistAndRetrieveTopic()
    {
        using var context = new MyDbContext(_options);
        var repo = new TopicRepository(context);

        var topic = new Topic { Id = "T1", Title = "Test", UserId = "U1" };

        await repo.CreateTopicAsync(topic);
        var result = await repo.GetTopicAsync("T1");

        Assert.NotNull(result);
        Assert.Equal("T1", result.Id);
    }

    [Fact]
    public async Task GetTopicAsync_ShouldReturnNull_WhenNotFound()
    {
        var emptyOptions = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(databaseName: "EmptyDb_" + Guid.NewGuid().ToString())
            .Options;

        using var context = new MyDbContext(emptyOptions);
        var repo = new TopicRepository(context);

        var result = await repo.GetTopicAsync("QualquerId");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetTopicsAsync_ShouldReturnAllTopics()
    {
        using var context = new MyDbContext(_options);

        context.Topics.AddRange(
            new Topic { Id = "1", Title = "T1", UserId = "U1" },
            new Topic { Id = "2", Title = "T2", UserId = "U2" }
        );

        await context.SaveChangesAsync();

        var repo = new TopicRepository(context);

        var result = await repo.GetTopicsAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CountIdeasByTopicIdAsync_ShouldReturnCorrectCount()
    {
        using var context = new MyDbContext(_options);

        context.Ideas.AddRange(
            new Idea { Id = "1", TopicId = "T1" },
            new Idea { Id = "2", TopicId = "T1" }
        );

        await context.SaveChangesAsync();

        var repo = new TopicRepository(context);
        var count = await repo.CountIdeasByTopicIdAsync("T1");

        Assert.Equal(2, count);
    }
}