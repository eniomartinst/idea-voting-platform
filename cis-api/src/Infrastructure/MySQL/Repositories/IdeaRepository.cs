using System.Diagnostics.CodeAnalysis;
using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Infrastructure.MySQL.Context;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Src.Infrastructure.MySQL.Repositories;

/// <summary>
/// MySQL implementation of <see cref="IIdeaRepository"/> for persisting and retrieving Idea entities.
/// </summary>
[ExcludeFromCodeCoverage]
public class IdeaRepository : IIdeaRepository
{
    private readonly MyDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdeaRepository"/> class.
    /// </summary>
    /// <param name="context">The database context used for data access.</param>
    public IdeaRepository(MyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Creates and persists a new idea in the database.
    /// </summary>
    /// <param name="idea">The idea entity to be created.</param>
    /// <returns>The persisted <see cref="Idea"/> entity.</returns>
    public async Task<Idea> CreateIdeaAsync(Idea idea)
    {
        await _context.Ideas.AddAsync(idea);
        await _context.SaveChangesAsync();
        return idea;
    }

    /// <summary>
    /// Retrieves all ideas associated with a specific topic.
    /// </summary>
    /// <param name="topicId">The identifier of the topic.</param>
    /// <returns>A collection of <see cref="Idea"/> entities linked to the topic.</returns>
    public async Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(string topicId)
    {
        return await _context.Ideas
            .Where(i => i.TopicId == topicId)
            .ToListAsync();
    }

    /// <summary>
    /// Retrieves an idea by its identifier.
    /// </summary>
    /// <param name="id">The idea identifier.</param>
    /// <returns>The matching <see cref="Idea"/> if found; otherwise, null.</returns>
    public async Task<Idea?> GetIdeaByIdAsync(string id)
    {
        return await _context.Ideas
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    /// <summary>
    /// Updates an existing idea in the database.
    /// </summary>
    public async Task<Idea> UpdateIdeaAsync(Idea idea)
    {
        _context.Ideas.Update(idea);
        await _context.SaveChangesAsync();
        return idea;
    }
}
