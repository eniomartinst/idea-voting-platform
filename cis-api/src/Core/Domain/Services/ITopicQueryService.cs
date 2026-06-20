using CisApi.Src.Core.Domain.Entities;

namespace CisApi.Src.Core.Domain.Services;

public interface ITopicQueryService
{
    Task<IEnumerable<Topic>> GetTopicsAsync();
    Task<Topic> GetTopicAsync(string id);
}