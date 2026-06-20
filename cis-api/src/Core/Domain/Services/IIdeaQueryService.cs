using CisApi.Src.Core.Domain.Entities;

namespace CisApi.Src.Core.Domain.Services;

public interface IIdeaQueryService
{
    Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(string topicId);
}