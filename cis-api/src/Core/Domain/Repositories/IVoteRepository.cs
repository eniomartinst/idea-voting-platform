namespace CisApi.Src.Core.Domain.Repositories;

public interface IVoteRepository
{
    Task<bool> ExistsAsync(string ideaId, string userId);

    Task AddAsync(string ideaId, string userId);

    Task RemoveAsync(string ideaId, string userId);

    Task<int> CountByIdeaIdAsync(string ideaId);

    Task<List<string>> GetUserIdsByIdeaIdAsync(string ideaId);
}