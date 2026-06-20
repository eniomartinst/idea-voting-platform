using CisApi.Src.Core.Application.Services;
using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Exceptions;
using CisApi.Src.Core.Domain.Repositories;
using Moq;

namespace CisApi.Test.Core.Application.Services;

public class IdeaQueryServiceTests
{
    [Fact]
    public async Task GetIdeasByTopicIdAsync_ReturnsIdeas()
    {
        var topicRepoMock = new Mock<ITopicRepository>();
        var ideaRepoMock = new Mock<IIdeaRepository>();

        var topicId = "topic-1";
        var topic = new Topic { Id = topicId, Title = "Test Topic" };

        var ideas = new List<Idea>
        {
            new Idea { Id = "idea-1", TopicId = topicId, UserId = "user-1", Content = "Great Idea" }
        };

        topicRepoMock.Setup(r => r.GetTopicAsync(topicId)).ReturnsAsync(topic);
        ideaRepoMock.Setup(r => r.GetIdeasByTopicIdAsync(topicId)).ReturnsAsync(ideas);

        var service = new IdeaQueryService(ideaRepoMock.Object, topicRepoMock.Object);

        var returnedIdeas = await service.GetIdeasByTopicIdAsync(topicId);

        Assert.Single(returnedIdeas);
        Assert.Equal("idea-1", returnedIdeas.First().Id);
    }

    [Fact]
    public async Task GetIdeasByTopicIdAsync_ThrowsNotFoundException_WhenTopicNotFound()
    {
        var topicRepoMock = new Mock<ITopicRepository>();
        var ideaRepoMock = new Mock<IIdeaRepository>();

        var topicId = "invalid-topic";

        topicRepoMock.Setup(r => r.GetTopicAsync(topicId)).ReturnsAsync((Topic?)null);

        var service = new IdeaQueryService(ideaRepoMock.Object, topicRepoMock.Object);

        await Assert.ThrowsAsync<NotFoundException>(() => service.GetIdeasByTopicIdAsync(topicId));
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenDependenciesAreNull()
    {
        var topicRepoMock = new Mock<ITopicRepository>();
        var ideaRepoMock = new Mock<IIdeaRepository>();

        Assert.Throws<ArgumentNullException>(() => new IdeaQueryService(null!, topicRepoMock.Object));
        Assert.Throws<ArgumentNullException>(() => new IdeaQueryService(ideaRepoMock.Object, null!));
    }
}