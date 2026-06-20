using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Presentation.RestApi.Dtos;
using CisApi.Src.Presentation.RestApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CisApi.Src.Presentation.RestApi.Controllers;

/// <summary>
/// Handles topic-related queries operations.
/// </summary>
[ApiController]
[Route("api/v1/topics")]
public class TopicQueryController : ControllerBase
{
    private readonly ITopicQueryService _service;
    private readonly IUserQueryService _userQueryService;
    private readonly IIdeaRepository _ideaRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicQueryController"/> class.
    /// </summary>
    /// <param name="service">Service responsible for topic queris.</param>
    /// <param name="userQueryService">Service used to retrieve user data from Users API.</param>
    /// <param name="ideaRepository">Repository responsible for retrieves ideas data</param>
    public TopicQueryController(
        ITopicQueryService service,
        IUserQueryService userQueryService,
        IIdeaRepository ideaRepository)
    {
        _service = service;
        _userQueryService = userQueryService;
        _ideaRepository = ideaRepository;
    }

    /// <summary>
    /// Retrieves all topics with their respective idea counts.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TopicResponseDto>>> GetTopicsAsync()
    {
        var topics = await _service.GetTopicsAsync();
        var response = new List<TopicResponseDto>();

        foreach (var topic in topics)
        {
            // Enriches data for each topic
            var user = await _userQueryService.GetByIdAsync(topic.UserId);
            var ideas = await _ideaRepository.GetIdeasByTopicIdAsync(topic.Id);

            response.Add(TopicMapper.ToResponse(topic, user, ideas.Count()));
        }

        return Ok(response);
    }
}