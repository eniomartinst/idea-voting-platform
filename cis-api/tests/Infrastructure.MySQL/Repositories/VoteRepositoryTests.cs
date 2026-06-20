using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Infrastructure.MySQL.Context;
using CisApi.Src.Infrastructure.MySQL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Test.Infrastructure.MySQL.Repositories;

public class VoteRepositoryTests
{
    private static MyDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<MyDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MyDbContext(options);
    }

    [Fact]
    public async Task AddAsync_Should_InsertVote()
    {
        var context = CreateContext();
        var repo = new VoteRepository(context);

        await repo.AddAsync("idea-1", "user-1");

        var vote = await context.Votes.FirstOrDefaultAsync();

        Assert.NotNull(vote);
        Assert.Equal("idea-1", vote!.IdeaId);
        Assert.Equal("user-1", vote.UserId);
    }

    [Fact]
    public async Task ExistsAsync_Should_ReturnTrue_WhenVoteExists()
    {
        var context = CreateContext();

        context.Votes.Add(new Vote
        {
            IdeaId = "idea-1",
            UserId = "user-1"
        });

        await context.SaveChangesAsync();

        var repo = new VoteRepository(context);

        var exists = await repo.ExistsAsync("idea-1", "user-1");

        Assert.True(exists);
    }

    [Fact]
    public async Task RemoveAsync_Should_DeleteVote_WhenExists()
    {
        var context = CreateContext();

        var vote = new Vote
        {
            IdeaId = "idea-1",
            UserId = "user-1"
        };

        context.Votes.Add(vote);
        await context.SaveChangesAsync();

        var repo = new VoteRepository(context);

        await repo.RemoveAsync("idea-1", "user-1");

        var exists = await context.Votes.AnyAsync();

        Assert.False(exists);
    }

    [Fact]
    public async Task CountByIdeaId_Should_ReturnCorrectCount()
    {
        var context = CreateContext();

        context.Votes.AddRange(
            new Vote { IdeaId = "idea-1", UserId = "u1" },
            new Vote { IdeaId = "idea-1", UserId = "u2" }
        );

        await context.SaveChangesAsync();

        var repo = new VoteRepository(context);

        var count = await repo.CountByIdeaIdAsync("idea-1");

        Assert.Equal(2, count);
    }

    [Fact]
    public async Task GetUserIdsByIdeaId_Should_ReturnUserIds()
    {
        var context = CreateContext();

        context.Votes.AddRange(
            new Vote { IdeaId = "idea-1", UserId = "u1" },
            new Vote { IdeaId = "idea-1", UserId = "u2" }
        );

        await context.SaveChangesAsync();

        var repo = new VoteRepository(context);

        var result = await repo.GetUserIdsByIdeaIdAsync("idea-1");

        Assert.Contains("u1", result);
        Assert.Contains("u2", result);
        Assert.Equal(2, result.Count);
    }
}