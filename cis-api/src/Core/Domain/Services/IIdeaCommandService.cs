using CisApi.Src.Core.Domain.Entities;

namespace CisApi.Src.Core.Domain.Services;

/// <summary>
/// Defines the contract for querying idea information.
/// </summary>

public interface IIdeaCommandService
{
    Task<Idea> CreateIdeaAsync(Idea idea);
    Task<Idea> VoteIdeaAsync(string ideaId, ICurrentUser currentUser);
    Task<Idea> UnvoteIdeaAsync(string ideaId, ICurrentUser currentUser);
}