using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Presentation.RestApi.Controllers;
using CisApi.Src.Presentation.RestApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CisApi.Test.Presentation.RestApi.Controllers;

public class IdeaQueryControllerTests
{
    [Fact]
    public async Task GetTopicIdeasAsync_ReturnsOk_WithIdeas()
    {
        var serviceMock = new Mock<IIdeaQueryService>();
        var userQueryMock = new Mock<IUserQueryService>();
        var voteServiceMock = new Mock<IVoteQueryService>();

        const string topicId = "topic-1";
        const string ideaId = "idea-1";
        const string userId = "user-1";

        var user = new UserReference(userId, "User One", "user1");

        var ideasList = new List<Idea>
        {
            new()
            {
                Id = ideaId,
                TopicId = topicId,
                UserId = userId,
                Content = "Great Idea",
                CreatedAt = DateTime.UtcNow
            }
        };

        serviceMock
            .Setup(s => s.GetIdeasByTopicIdAsync(topicId))
            .ReturnsAsync(ideasList);

        userQueryMock
            .Setup(u => u.GetByIdAsync(userId))
            .ReturnsAsync(user);

        voteServiceMock
            .Setup(v => v.GetLoginsByIdeaIdAsync(ideaId))
            .ReturnsAsync(new List<string> { "user1" });

        var controller = new IdeaQueryController(
            serviceMock.Object,
            userQueryMock.Object,
            voteServiceMock.Object);

        var result = await controller.GetTopicIdeasAsync(topicId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var responseList = Assert.IsAssignableFrom<IEnumerable<IdeaResponseDto>>(okResult.Value);

        Assert.Single(responseList);

        var firstIdea = responseList.First();

        Assert.Equal(ideaId, firstIdea.Id);
        Assert.NotNull(firstIdea.CreatedBy);
        Assert.Equal("User One", firstIdea.CreatedBy!.Name);
        Assert.Contains("user1", firstIdea.VotedBy);
    }

    [Fact]
    public void Constructor_ThrowsArgumentNullException_WhenDependenciesAreNull()
    {
        var serviceMock = new Mock<IIdeaQueryService>();
        var userQueryMock = new Mock<IUserQueryService>();
        var voteServiceMock = new Mock<IVoteQueryService>();

        Assert.Throws<ArgumentNullException>(() =>
            new IdeaQueryController(null!, userQueryMock.Object, voteServiceMock.Object));

        Assert.Throws<ArgumentNullException>(() =>
            new IdeaQueryController(serviceMock.Object, null!, voteServiceMock.Object));

        Assert.Throws<ArgumentNullException>(() =>
            new IdeaQueryController(serviceMock.Object, userQueryMock.Object, null!));
    }
}