using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Infrastructure.MySQL.Context;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Src.Infrastructure.MySQL.Repositories;

public class VoteRepository : IVoteRepository
{
    private readonly MyDbContext _context;

    public VoteRepository(MyDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<bool> ExistsAsync(string ideaId, string userId)
    {
        return await _context.Votes
            .AnyAsync(v => v.IdeaId == ideaId && v.UserId == userId);
    }

    public async Task AddAsync(string ideaId, string userId)
    {
        var vote = new Vote
        {
            IdeaId = ideaId,
            UserId = userId
        };

        _context.Votes.Add(vote);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveAsync(string ideaId, string userId)
    {
        var vote = await _context.Votes
            .FirstOrDefaultAsync(v => v.IdeaId == ideaId && v.UserId == userId);

        if (vote is null) return;

        _context.Votes.Remove(vote);
        await _context.SaveChangesAsync();
    }

    public async Task<int> CountByIdeaIdAsync(string ideaId)
    {
        return await _context.Votes
            .CountAsync(v => v.IdeaId == ideaId);
    }

    public async Task<List<string>> GetUserIdsByIdeaIdAsync(string ideaId)
    {
        return await _context.Votes
            .Where(v => v.IdeaId == ideaId)
            .Select(v => v.UserId)
            .ToListAsync();
    }
}