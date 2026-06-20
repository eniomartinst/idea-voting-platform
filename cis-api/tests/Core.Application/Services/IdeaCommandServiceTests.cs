using CisApi.Src.Core.Application.Services;
using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Exceptions;
using CisApi.Src.Core.Domain.Services;
using Moq;

namespace CisApi.Test.Core.Application.Services;

public class IdeaCommandServiceTests
{
    private readonly Mock<IIdeaRepository> _ideaRepo = new();
    private readonly Mock<ITopicRepository> _topicRepo = new();
    private readonly Mock<IVoteRepository> _voteRepo = new();
    private readonly IdeaCommandService _service;

    public IdeaCommandServiceTests()
    {
        _service = new IdeaCommandService(
            _ideaRepo.Object,
            _topicRepo.Object,
            _voteRepo.Object
        );
    }

    [Fact]
    public async Task Should_Create_Idea_When_Topic_Exists()
    {
        var idea = new Idea { TopicId = "topic-1" };
        _topicRepo.Setup(x => x.GetTopicAsync("topic-1")).ReturnsAsync(new Topic());
        _ideaRepo.Setup(x => x.CreateIdeaAsync(It.IsAny<Idea>())).ReturnsAsync(idea);

        var result = await _service.CreateIdeaAsync(idea);

        Assert.NotNull(result);
        _ideaRepo.Verify(x => x.CreateIdeaAsync(It.IsAny<Idea>()), Times.Once);
    }

    [Fact]
    public async Task Should_Throw_When_Topic_Not_Found()
    {
        var idea = new Idea { TopicId = "invalid" };
        _topicRepo.Setup(x => x.GetTopicAsync("invalid")).ReturnsAsync((Topic?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.CreateIdeaAsync(idea));
    }

    [Fact]
    public async Task VoteIdeaAsync_Should_Work()
    {
        var currentUserMock = new Mock<ICurrentUser>();
        currentUserMock.Setup(x => x.Id).Returns("user-2");

        var idea = new Idea { Id = "idea-1", UserId = "user-1" };

        _ideaRepo.Setup(x => x.GetIdeaByIdAsync("idea-1")).ReturnsAsync(idea);
        _voteRepo.Setup(x => x.ExistsAsync("idea-1", "user-2")).ReturnsAsync(false);
        _voteRepo.Setup(x => x.CountByIdeaIdAsync("idea-1")).ReturnsAsync(1);
        _ideaRepo.Setup(x => x.UpdateIdeaAsync(It.IsAny<Idea>())).ReturnsAsync(idea);

        var result = await _service.VoteIdeaAsync("idea-1", currentUserMock.Object);

        Assert.Equal(1, result.VotesCount);
        _voteRepo.Verify(x => x.AddAsync("idea-1", "user-2"), Times.Once);
    }

    [Fact]
    public async Task VoteIdeaAsync_Should_Throw_When_Idea_Not_Found()
    {
        var currentUserMock = new Mock<ICurrentUser>();
        _ideaRepo.Setup(x => x.GetIdeaByIdAsync("invalid")).ReturnsAsync((Idea?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.VoteIdeaAsync("invalid", currentUserMock.Object));
    }

    [Fact]
    public async Task VoteIdeaAsync_Should_Throw_When_User_Is_Owner()
    {
        var currentUserMock = new Mock<ICurrentUser>();
        currentUserMock.Setup(x => x.Id).Returns("user-1");

        var idea = new Idea { Id = "idea-1", UserId = "user-1" };
        _ideaRepo.Setup(x => x.GetIdeaByIdAsync("idea-1")).ReturnsAsync(idea);

        await Assert.ThrowsAsync<OperationNotAllowedException>(() => _service.VoteIdeaAsync("idea-1", currentUserMock.Object));
    }

    [Fact]
    public async Task VoteIdeaAsync_Should_Throw_When_Already_Voted()
    {
        var currentUserMock = new Mock<ICurrentUser>();
        currentUserMock.Setup(x => x.Id).Returns("user-2");

        var idea = new Idea { Id = "idea-1", UserId = "user-1" };

        _ideaRepo.Setup(x => x.GetIdeaByIdAsync("idea-1")).ReturnsAsync(idea);
        _voteRepo.Setup(x => x.ExistsAsync("idea-1", "user-2")).ReturnsAsync(true);

        await Assert.ThrowsAsync<OperationNotAllowedException>(() =>
            _service.VoteIdeaAsync("idea-1", currentUserMock.Object));
    }

    [Fact]
    public async Task UnvoteIdeaAsync_Should_Work()
    {
        var currentUserMock = new Mock<ICurrentUser>();
        currentUserMock.Setup(x => x.Id).Returns("user-2");

        var idea = new Idea { Id = "idea-1", UserId = "user-1" };

        _ideaRepo.Setup(x => x.GetIdeaByIdAsync("idea-1")).ReturnsAsync(idea);
        _voteRepo.Setup(x => x.ExistsAsync("idea-1", "user-2")).ReturnsAsync(true);
        _voteRepo.Setup(x => x.CountByIdeaIdAsync("idea-1")).ReturnsAsync(0);
        _ideaRepo.Setup(x => x.UpdateIdeaAsync(It.IsAny<Idea>())).ReturnsAsync(idea);

        var result = await _service.UnvoteIdeaAsync("idea-1", currentUserMock.Object);

        Assert.Equal(0, result.VotesCount);
        _voteRepo.Verify(x => x.RemoveAsync("idea-1", "user-2"), Times.Once);
    }

    [Fact]
    public async Task UnvoteIdeaAsync_Should_Throw_When_Idea_Not_Found()
    {
        var currentUserMock = new Mock<ICurrentUser>();
        _ideaRepo.Setup(x => x.GetIdeaByIdAsync("invalid")).ReturnsAsync((Idea?)null);

        await Assert.ThrowsAsync<NotFoundException>(() => _service.UnvoteIdeaAsync("invalid", currentUserMock.Object));
    }

    [Fact]
    public async Task UnvoteIdeaAsync_Should_Throw_When_User_Has_Not_Voted()
    {
        var currentUserMock = new Mock<ICurrentUser>();
        currentUserMock.Setup(x => x.Id).Returns("user-2");

        var idea = new Idea { Id = "idea-1", UserId = "user-1" };

        _ideaRepo.Setup(x => x.GetIdeaByIdAsync("idea-1"))
            .ReturnsAsync(idea);

        _voteRepo.Setup(x => x.ExistsAsync("idea-1", "user-2"))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<OperationNotAllowedException>(() =>
            _service.UnvoteIdeaAsync("idea-1", currentUserMock.Object));
    }
}