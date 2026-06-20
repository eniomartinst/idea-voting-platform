using System.Diagnostics.CodeAnalysis;
using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Infrastructure.MySQL.Context;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Src.Infrastructure.MySQL.Repositories;

/// <summary>
/// MySQL implementation of <see cref="ITopicRepository"/> for persisting and retrieving Topic entities.
/// </summary>
[ExcludeFromCodeCoverage]
public class TopicRepository : ITopicRepository
{
    private readonly MyDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TopicRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    public TopicRepository(MyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates and persists a new topic in the database.
    /// </summary>
    /// <param name="topic">The topic entity to be created.</param>
    /// <returns>The persisted <see cref="Topic"/> entity.</returns>
    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        await _context.Topics.AddAsync(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    /// <summary>
    /// Counts the number of ideas associated with a specific topic in the database.
    /// </summary>
    /// <param name="topicId">The unique identifier of the topic.</param>
    /// <returns>The total number of ideas linked to the specified topic.</returns>
    public async Task<int> CountIdeasByTopicIdAsync(string topicId)
    {
        return await _context.Ideas
            .CountAsync(i => i.TopicId == topicId);
    }

    /// <summary>
    /// Checks whether a topic exists by its identifier.
    /// </summary>
    /// <param name="topicId">The topic identifier.</param>
    /// <returns>True if the topic exists; otherwise, false.</returns>
    public async Task<bool> ExistsByIdAsync(string topicId)
    {
        return await _context.Topics
            .AnyAsync(t => t.Id == topicId);
    }

    /// <summary>
    /// Retrieves all topics from the database.
    /// </summary>
    /// <returns>A collection of <see cref="Topic"/> entities.</returns>
    public async Task<IEnumerable<Topic>> GetTopicsAsync()
    {
        // Return all tópics
        return await _context.Topics.ToListAsync();
    }

    /// <summary>
    /// Retrieves a topic by its identifier.
    /// </summary>
    /// <param name="id">The topic identifier.</param>
    /// <returns>The matching <see cref="Topic"/> if found; otherwise, null.</returns>
    public async Task<Topic?> GetTopicAsync(string id)
    {
        return await _context.Topics.FirstOrDefaultAsync(t => t.Id == id);
    }
}
