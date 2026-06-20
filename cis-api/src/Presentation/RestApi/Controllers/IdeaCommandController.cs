using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Presentation.RestApi.Dtos;
using CisApi.Src.Presentation.RestApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CisApi.Src.Presentation.RestApi.Controllers;

/// <summary>
/// Handles idea-related command operations.
/// </summary>
[ApiController]
public class IdeaCommandController : ControllerBase
{
    private readonly IIdeaCommandService _service;
    private readonly ICurrentUser _currentUser;
    private readonly IUserQueryService _userQueryService;
    private readonly ITopicRepository _topicRepository;
    private readonly IVoteQueryService _voteQueryService;

    public IdeaCommandController(
        IIdeaCommandService service,
        ICurrentUser currentUser,
        IUserQueryService userQueryService,
        ITopicRepository topicRepository,
        IVoteQueryService voteQueryService)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _currentUser = currentUser ?? throw new ArgumentNullException(nameof(currentUser));
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
        _voteQueryService = voteQueryService ?? throw new ArgumentNullException(nameof(voteQueryService));
    }

    /// <summary>
    /// Creates a new idea under a specific topic.
    /// </summary>
    /// <param name="topicId">The topic identifier.</param>
    /// <param name="request">The idea creation request payload.</param>
    /// <returns>The created idea.</returns>
    [HttpPost("api/v1/topics/{topicId}/ideas")]
    public async Task<ActionResult<IdeaResponseDto>> CreateIdeaAsync(
        [FromRoute] string topicId,
        [FromBody] IdeaRequestDto request)
    {
        var idea = IdeaMapper.ToDomain(request, topicId, _currentUser);
        var result = await _service.CreateIdeaAsync(idea);

        var votedBy = await _voteQueryService.GetLoginsByIdeaIdAsync(result.Id);
        var user = await _userQueryService.GetByIdAsync(result.UserId);

        return StatusCode(201, IdeaMapper.ToResponse(result, request.Content, user, votedBy));
    }

    /// <summary>
    /// Votes for an existing idea.
    /// </summary>
    /// <param name="ideaId">The idea identifier.</param>
    /// <returns>The updated idea with the new vote count.</returns>
    [HttpPost("/api/v1/ideas/{ideaId}/vote")]
    public async Task<ActionResult<IdeaResponseDto>> VoteIdeaAsync([FromRoute] string ideaId)
    {
        var result = await _service.VoteIdeaAsync(ideaId, _currentUser);

        var topic = await _topicRepository.GetTopicAsync(result.TopicId);
        var user = await _userQueryService.GetByIdAsync(result.UserId);

        var votedBy = await _voteQueryService.GetLoginsByIdeaIdAsync(result.Id);

        return Ok(IdeaMapper.ToResponse(result, topic?.Title ?? "", user, votedBy));
    }

    /// <summary>
    /// Un-votes an existing idea.
    /// </summary>
    /// <param name="ideaId">The idea identifier.</param>
    /// <returns>The updated idea with the decreased vote count.</returns>
    [HttpPost("/api/v1/ideas/{ideaId}/unvote")]
    public async Task<ActionResult<IdeaResponseDto>> UnvoteIdeaAsync([FromRoute] string ideaId)
    {
        var result = await _service.UnvoteIdeaAsync(ideaId, _currentUser);

        var topic = await _topicRepository.GetTopicAsync(result.TopicId);
        var user = await _userQueryService.GetByIdAsync(result.UserId);

        var votedBy = await _voteQueryService.GetLoginsByIdeaIdAsync(result.Id);

        return Ok(IdeaMapper.ToResponse(result, topic?.Title ?? "", user, votedBy));
    }
}
