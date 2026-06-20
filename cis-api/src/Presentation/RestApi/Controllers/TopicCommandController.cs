using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Presentation.RestApi.Dtos;
using CisApi.Src.Presentation.RestApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace CisApi.Src.Presentation.RestApi.Controllers;

[ApiController]
[Route("api/v1/topics")]
public class TopicCommandController : ControllerBase
{
    private readonly ITopicCommandService _service;
    private readonly ICurrentUser _currentUser;
    private readonly IUserQueryService _userQueryService;

    public TopicCommandController(ITopicCommandService service, ICurrentUser currentUser, IUserQueryService userQueryService)
    {
        _service = service;
        _currentUser = currentUser;
        _userQueryService = userQueryService;
    }

    [HttpPost]
    public async Task<ActionResult<TopicResponseDto>> CreateTopicAsync([FromBody] TopicRequestDto request)
    {
        var topic = TopicMapper.ToDomain(request, _currentUser);
        var result = await _service.CreateTopicAsync(topic);

        var user = await _userQueryService.GetByIdAsync(result.UserId);
        return StatusCode(201, TopicMapper.ToResponse(result, user, 0));
    }
}