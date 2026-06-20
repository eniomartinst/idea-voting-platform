using CisApi.Src.Core.Domain.Entities;

namespace CisApi.Src.Core.Domain.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Idea"/> entities.
/// </summary>
public interface IIdeaRepository
{
    /// <summary>
    /// Persists a new idea in the database.
    /// </summary>
    /// <param name="idea">The idea entity to be created.</param>
    /// <returns>The persisted <see cref="Idea"/> entity.</returns>
    Task<Idea> CreateIdeaAsync(Idea idea);

    /// <summary>
    /// Retrieves all ideas associated with a specific topic.
    /// </summary>
    /// <param name="topicId">The topic identifier.</param>
    /// <returns>A collection of <see cref="Idea"/> entities.</returns>
    Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(string topicId);

    /// <summary>
    /// Retrieves an idea by its identifier.
    /// </summary>
    /// <param name="id">The idea identifier.</param>
    /// <returns>The matching <see cref="Idea"/> if found; otherwise, null.</returns>
    Task<Idea?> GetIdeaByIdAsync(string id);

    Task<Idea> UpdateIdeaAsync(Idea idea);
}