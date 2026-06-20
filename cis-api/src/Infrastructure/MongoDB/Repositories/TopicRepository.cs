using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using MongoDB.Driver;

namespace CisApi.Src.Infrastructure.MongoDB.Repositories;

public class TopicRepository : ITopicRepository
{
    private readonly IMongoCollection<Topic> _topicsCollection;
    private readonly IMongoCollection<Idea> _ideasCollection;

    public TopicRepository(IMongoDatabase database)
    {
        _topicsCollection = database.GetCollection<Topic>("topics");
        _ideasCollection = database.GetCollection<Idea>("ideas");
    }

    public async Task<int> CountIdeasByTopicIdAsync(string topicId)
    {
        var count = await _ideasCollection.CountDocumentsAsync(i => i.TopicId == topicId);
        return (int)count;
    }

    public async Task<Topic> CreateTopicAsync(Topic topic)
    {
        if (string.IsNullOrEmpty(topic.Id))
        {
            topic.Id = Guid.NewGuid().ToString();
        }
        await _topicsCollection.InsertOneAsync(topic);
        return topic;
    }

    public async Task<bool> ExistsByIdAsync(string topicId)
    {
        var count = await _topicsCollection.CountDocumentsAsync(t => t.Id == topicId);
        return count > 0;
    }

    public async Task<Topic?> GetTopicAsync(string id)
    {
        var cursor = await _topicsCollection.FindAsync(t => t.Id == id);
        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Topic>> GetTopicsAsync()
    {
        var cursor = await _topicsCollection.FindAsync(Builders<Topic>.Filter.Empty);
        return await cursor.ToListAsync();
    }
}
