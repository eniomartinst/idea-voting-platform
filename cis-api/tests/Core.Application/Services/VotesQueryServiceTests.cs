using CisApi.Src.Core.Application.Services;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.ValueObjects;
using Moq;

namespace CisApi.Test.Core.Application.Services;

public class VoteQueryServiceTests
{
    private readonly Mock<IVoteRepository> _voteRepository = new();
    private readonly Mock<IUserQueryService> _userQueryService = new();

    private readonly VoteQueryService _service;

    public VoteQueryServiceTests()
    {
        _service = new VoteQueryService(_voteRepository.Object, _userQueryService.Object);
    }

    [Fact]
    public async Task Should_Return_Logins_When_Users_Exist()
    {
        // Arrange
        var ideaId = "idea-1";

        _voteRepository.Setup(x => x.GetUserIdsByIdeaIdAsync(ideaId))
            .ReturnsAsync(new List<string> { "u1", "u2" });

        _userQueryService.Setup(x => x.GetByIdAsync("u1"))
            .ReturnsAsync(new UserReference("u1", "User One", "user1"));

        _userQueryService.Setup(x => x.GetByIdAsync("u2"))
            .ReturnsAsync(new UserReference("u2", "User Two", "user2"));

        // Act
        var result = await _service.GetLoginsByIdeaIdAsync(ideaId);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Contains("user1", result);
        Assert.Contains("user2", result);
    }

    [Fact]
    public async Task Should_Ignore_Null_Users()
    {
        // Arrange
        var ideaId = "idea-1";

        _voteRepository.Setup(x => x.GetUserIdsByIdeaIdAsync(ideaId))
            .ReturnsAsync(new List<string> { "u1", "u2" });

        _userQueryService.Setup(x => x.GetByIdAsync("u1"))
            .ReturnsAsync(new UserReference("u1", "User One", "user1"));

        _userQueryService.Setup(x => x.GetByIdAsync("u2"))
            .ReturnsAsync((UserReference?)null);

        // Act
        var result = await _service.GetLoginsByIdeaIdAsync(ideaId);

        // Assert
        Assert.Single(result);
        Assert.Contains("user1", result);
    }

    [Fact]
    public async Task Should_Return_Empty_List_When_No_Votes()
    {
        // Arrange
        var ideaId = "idea-1";

        _voteRepository.Setup(x => x.GetUserIdsByIdeaIdAsync(ideaId)).ReturnsAsync([]);

        // Act
        var result = await _service.GetLoginsByIdeaIdAsync(ideaId);

        // Assert
        Assert.Empty(result);
    }
}