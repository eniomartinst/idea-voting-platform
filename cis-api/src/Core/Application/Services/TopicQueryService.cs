using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Exceptions;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;

namespace CisApi.Src.Core.Application.Services;

/// <summary>
/// Service responsible for querying topics without business logic modification.
/// </summary>
public class TopicQueryService : ITopicQueryService
{
    private readonly ITopicRepository _repository;

    public TopicQueryService(ITopicRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<IEnumerable<Topic>> GetTopicsAsync()
    {
        // Returns all topics from the repository
        return await _repository.GetTopicsAsync();
    }

    public async Task<Topic> GetTopicAsync(string id)
    {
        var topic = await _repository.GetTopicAsync(id);

        // Using our standardized NotFoundException
        if (topic is null) throw new NotFoundException("Topic", id);

        return topic;
    }
}