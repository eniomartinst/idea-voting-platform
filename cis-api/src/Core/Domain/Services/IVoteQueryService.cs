namespace CisApi.Src.Core.Domain.Services;

public interface IVoteQueryService
{
    Task<List<string>> GetLoginsByIdeaIdAsync(string ideaId);
}