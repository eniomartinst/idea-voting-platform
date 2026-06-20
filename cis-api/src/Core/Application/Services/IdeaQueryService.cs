using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Exceptions;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;

namespace CisApi.Src.Core.Application.Services;

/// <summary>
/// Service responsible for querying ideas.
/// </summary>
public class IdeaQueryService : IIdeaQueryService
{
    private readonly IIdeaRepository _repository;
    private readonly ITopicRepository _topicRepository;

    public IdeaQueryService(IIdeaRepository repository, ITopicRepository topicRepository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _topicRepository = topicRepository ?? throw new ArgumentNullException(nameof(topicRepository));
    }

    public async Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(string topicId)
    {
        // Validation: Does the topic exist?
        var topic = await _topicRepository.GetTopicAsync(topicId);
        if (topic is null) throw new NotFoundException("Topic", topicId);

        return await _repository.GetIdeasByTopicIdAsync(topicId);
    }
}