using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Exceptions;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;

namespace CisApi.Src.Core.Application.Services;

public class TopicCommandService : ITopicCommandService
{
    private readonly ITopicRepository _repository;

    public TopicCommandService(ITopicRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        return await _repository.CreateTopicAsync(topic);
    }
}