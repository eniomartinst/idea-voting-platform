using CisApi.Src.Core.Domain.Entities;

namespace CisApi.Test.Core.Domain.Entities;

public class DomainEntitiesTests
{
    [Fact]
    public void Entities_Should_SetAndGetProperties()
    {
        // Assert Idea
        var idea = new Idea
        {
            Id = "i1",
            TopicId = "t1",
            Content = "Idea Title",
            CreatedAt = DateTime.UtcNow,
            UserId = "user1",
            VotesCount = 5
        };

        Assert.Equal("i1", idea.Id);
        Assert.Equal("Idea Title", idea.Content);
        Assert.Equal(5, idea.VotesCount);

        // Assert Topic
        var topic = new Topic
        {
            Id = "t1",
            Title = "Topic Title",
            Description = "Topic Desc",
            CreatedAt = DateTime.UtcNow,
            UserId = "user1"
        };

        topic.Ideas.Add(idea);

        Assert.Equal("t1", topic.Id);
        Assert.Equal("Topic Title", topic.Title);
        Assert.Single(topic.Ideas);
    }

    [Fact]
    public void Vote_Should_SetAndGetProperties()
    {
        // Assert Votes
        var vote = new Vote
        {
            IdeaId = "idea-1",
            UserId = "user-1",
            CreatedAt = DateTime.UtcNow
        };

        Assert.Equal("idea-1", vote.IdeaId);
        Assert.Equal("user-1", vote.UserId);
        Assert.True(vote.CreatedAt <= DateTime.UtcNow);
    }

    [Fact]
    public void Entities_Should_HaveValidDefaultState()
    {
        var topic = new Topic();
        var idea = new Idea();

        Assert.Empty(topic.Id);
        Assert.Empty(topic.Ideas);
        Assert.Empty(topic.UserId);

        Assert.Empty(idea.Id);
        Assert.Empty(idea.Content);
        Assert.Equal(0, idea.VotesCount);
    }
}