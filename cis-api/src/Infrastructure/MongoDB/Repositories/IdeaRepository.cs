using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using MongoDB.Driver;

namespace CisApi.Src.Infrastructure.MongoDB.Repositories;

public class IdeaRepository : IIdeaRepository
{
    private readonly IMongoCollection<Idea> _ideasCollection;

    public IdeaRepository(IMongoDatabase database)
    {
        _ideasCollection = database.GetCollection<Idea>("ideas");
    }

    public async Task<Idea> CreateIdeaAsync(Idea idea)
    {
        if (string.IsNullOrEmpty(idea.Id))
        {
            idea.Id = Guid.NewGuid().ToString();
        }
        await _ideasCollection.InsertOneAsync(idea);
        return idea;
    }

    public async Task<Idea?> GetIdeaByIdAsync(string id)
    {
        var cursor = await _ideasCollection.FindAsync(i => i.Id == id);
        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Idea>> GetIdeasByTopicIdAsync(string topicId)
    {
        var cursor = await _ideasCollection.FindAsync(i => i.TopicId == topicId);
        return await cursor.ToListAsync();
    }

    public async Task<Idea> UpdateIdeaAsync(Idea idea)
    {
        await _ideasCollection.ReplaceOneAsync(i => i.Id == idea.Id, idea);
        return idea;
    }
}
