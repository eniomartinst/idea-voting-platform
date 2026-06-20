using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Presentation.RestApi.Controllers;
using CisApi.Src.Presentation.RestApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CisApi.Test.Presentation.RestApi.Controllers;

public class IdeaCommandControllerTests
{
    private readonly Mock<IIdeaCommandService> _service = new();
    private readonly Mock<ICurrentUser> _currentUser = new();
    private readonly Mock<IUserQueryService> _userService = new();
    private readonly Mock<ITopicRepository> _topicRepository = new();
    private readonly Mock<IVoteQueryService> _voteQueryService = new();

    private readonly IdeaCommandController _controller;

    public IdeaCommandControllerTests()
    {
        _controller = new IdeaCommandController(
            _service.Object,
            _currentUser.Object,
            _userService.Object,
            _topicRepository.Object,
            _voteQueryService.Object);
    }

    [Fact]
    public async Task Should_Return_Created_When_Success()
    {
        var topicId = "t1";
        var request = new IdeaRequestDto { Content = "watch movies" };

        var idea = new Idea
        {
            Id = "1",
            TopicId = topicId,
            UserId = "u1",
            Content = "watch movies",
            VotesCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        var userReference = new UserReference("u1", "javier", "javier");

        _service.Setup(x => x.CreateIdeaAsync(It.IsAny<Idea>())).ReturnsAsync(idea);
        _userService.Setup(x => x.GetByIdAsync("u1")).ReturnsAsync(userReference);
        _voteQueryService.Setup(x => x.GetLoginsByIdeaIdAsync("1")).ReturnsAsync(new List<string>());

        var result = await _controller.CreateIdeaAsync(topicId, request);

        var createdResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(201, createdResult.StatusCode);

        var dto = Assert.IsType<IdeaResponseDto>(createdResult.Value);
        Assert.Equal("1", dto.Id);
    }

    [Fact]
    public async Task VoteIdeaAsync_Should_Return_Ok_When_Success()
    {
        var ideaId = "1";

        var idea = new Idea
        {
            Id = ideaId,
            TopicId = "t1",
            UserId = "u1",
            VotesCount = 1,
            Content = "watch movies"
        };

        var topic = new Topic
        {
            Id = "t1",
            Title = "topic title"
        };

        var userReference = new UserReference("u1", "javier", "javier");

        _service.Setup(x => x.VoteIdeaAsync(ideaId, It.IsAny<ICurrentUser>()))
            .ReturnsAsync(idea);

        _topicRepository.Setup(x => x.GetTopicAsync("t1")).ReturnsAsync(topic);
        _userService.Setup(x => x.GetByIdAsync("u1")).ReturnsAsync(userReference);
        _voteQueryService.Setup(x => x.GetLoginsByIdeaIdAsync("1"))
            .ReturnsAsync(new List<string> { "javier" });

        var result = await _controller.VoteIdeaAsync(ideaId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var dto = Assert.IsType<IdeaResponseDto>(okResult.Value);
        Assert.Equal("topic title", dto.Topic.Title);
    }

    [Fact]
    public async Task UnvoteIdeaAsync_Should_Return_Ok_When_Success()
    {
        var ideaId = "1";

        var idea = new Idea
        {
            Id = ideaId,
            TopicId = "t1",
            UserId = "u1",
            VotesCount = 0,
            Content = "watch movies"
        };

        var topic = new Topic
        {
            Id = "t1",
            Title = "topic title"
        };

        var userReference = new UserReference("u1", "javier", "javier");

        _service.Setup(x => x.UnvoteIdeaAsync(ideaId, It.IsAny<ICurrentUser>()))
            .ReturnsAsync(idea);

        _topicRepository.Setup(x => x.GetTopicAsync("t1")).ReturnsAsync(topic);
        _userService.Setup(x => x.GetByIdAsync("u1")).ReturnsAsync(userReference);
        _voteQueryService.Setup(x => x.GetLoginsByIdeaIdAsync("1")).ReturnsAsync([]);

        var result = await _controller.UnvoteIdeaAsync(ideaId);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        Assert.Equal(200, okResult.StatusCode);

        var dto = Assert.IsType<IdeaResponseDto>(okResult.Value);
        Assert.Equal("topic title", dto.Topic.Title);
    }
}