using CisApi.Src.Core.Domain.Entities;

namespace CisApi.Src.Core.Domain.Services;

/// <summary>
/// Defines command operations for <see cref="Topic"/> domain actions.
/// </summary>
public interface ITopicCommandService
{
    /// <summary>
    /// Creates a new topic in the system.
    /// </summary>
    /// <param name="topic">The topic to be created.</param>
    /// <returns>The created <see cref="Topic"/> entity.</returns>
    Task<Topic> CreateTopicAsync(Topic topic);
}