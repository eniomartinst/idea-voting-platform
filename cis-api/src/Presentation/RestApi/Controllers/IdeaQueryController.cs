using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Presentation.RestApi.Dtos;
using CisApi.Src.Presentation.RestApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CisApi.Src.Presentation.RestApi.Controllers;

/// <summary>
/// Handles idea-related query operations.
/// </summary>
[ApiController]
[Route("api/v1/topics")]
public class IdeaQueryController : ControllerBase
{
    private readonly IIdeaQueryService _service;
    private readonly IUserQueryService _userQueryService;
    private readonly IVoteQueryService _voteQueryService;

    public IdeaQueryController(
        IIdeaQueryService service,
        IUserQueryService userQueryService,
        IVoteQueryService voteQueryService)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
        _voteQueryService = voteQueryService ?? throw new ArgumentNullException(nameof(voteQueryService));
    }

    /// <summary>
    /// Retrieves all ideas associated with a specific topic.
    /// </summary>
    /// <param name="topicId">The topic identifier.</param>
    /// <returns>A collection of ideas.</returns>
    [HttpGet("{topicId}/ideas")]
    public async Task<ActionResult<IEnumerable<IdeaResponseDto>>> GetTopicIdeasAsync([FromRoute] string topicId)
    {
        var ideas = await _service.GetIdeasByTopicIdAsync(topicId);

        var responseList = new List<IdeaResponseDto>();

        foreach (var idea in ideas)
        {
            var user = await _userQueryService.GetByIdAsync(idea.UserId);
            var votedBy = await _voteQueryService.GetLoginsByIdeaIdAsync(idea.Id);

            responseList.Add(IdeaMapper.ToResponse(idea, idea.Content, user, votedBy));
        }

        return Ok(responseList);
    }
}