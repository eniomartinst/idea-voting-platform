using CisApi.Src.Core.Domain.Entities;

namespace CisApi.Src.Core.Domain.Repositories;

/// <summary>
/// Defines persistence operations for <see cref="Topic"/> entities.
/// </summary>
public interface ITopicRepository
{
    /// <summary>
    /// Persists a new topic in the database.
    /// </summary>
    /// <param name="topic">The topic entity to be created.</param>
    /// <returns>The persisted <see cref="Topic"/> entity.</returns>
    Task<Topic> CreateTopicAsync(Topic topic);

    /// <summary>
    /// Counts the number of ideas associated with a specific topic.
    /// </summary>
    /// <param name="topicId">The unique identifier of the topic.</param>
    /// <returns>The total number of ideas linked to the specified topic.</returns
    Task<int> CountIdeasByTopicIdAsync(string topicId);

    /// <summary>
    /// Checks whether a topic exists by its identifier.
    /// </summary>
    /// <param name="topicId">The topic identifier.</param>
    /// <returns>True if the topic exists; otherwise, false.</returns>
    Task<bool> ExistsByIdAsync(string topicId);

    /// <summary>
    /// Retrieves all topics from the database.
    /// </summary>
    /// <returns>A collection of <see cref="Topic"/> entities.</returns>
    Task<IEnumerable<Topic>> GetTopicsAsync();

    /// <summary>
    /// Retrieves a topic by its identifier.
    /// </summary>
    /// <param name="id">The topic identifier.</param>
    /// <returns>The matching <see cref="Topic"/> if found; otherwise, null.</returns>
    Task<Topic?> GetTopicAsync(string id);
}
