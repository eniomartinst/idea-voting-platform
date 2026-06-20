using CisApi.Src.Core.Domain.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CisApi.Src.Infrastructure.MongoDB.Repositories;

public class VoteRepository : IVoteRepository
{
    private readonly IMongoCollection<BsonDocument> _votesCollection;

    public VoteRepository(IMongoDatabase database)
    {
        _votesCollection = database.GetCollection<BsonDocument>("votes");
    }

    public async Task<bool> ExistsAsync(string ideaId, string userId)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("ideaId", ideaId),
            Builders<BsonDocument>.Filter.Eq("userId", userId)
        );

        return await _votesCollection.Find(filter).AnyAsync();
    }

    public async Task AddAsync(string ideaId, string userId)
    {
        var doc = new BsonDocument
        {
            { "ideaId", ideaId },
            { "userId", userId },
            { "createdAt", DateTime.UtcNow }
        };

        // Compound Unique Index
        await _votesCollection.ReplaceOneAsync(
            Builders<BsonDocument>.Filter.Eq("_id", $"{ideaId}:{userId}"),
            doc,
            new ReplaceOptions { IsUpsert = true }
        );
    }

    public async Task RemoveAsync(string ideaId, string userId)
    {
        var filter = Builders<BsonDocument>.Filter.And(
            Builders<BsonDocument>.Filter.Eq("ideaId", ideaId),
            Builders<BsonDocument>.Filter.Eq("userId", userId)
        );

        await _votesCollection.DeleteOneAsync(filter);
    }

    public async Task<int> CountByIdeaIdAsync(string ideaId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("ideaId", ideaId);
        return (int)await _votesCollection.CountDocumentsAsync(filter);
    }

    public async Task<List<string>> GetUserIdsByIdeaIdAsync(string ideaId)
    {
        var filter = Builders<BsonDocument>.Filter.Eq("ideaId", ideaId);

        var votes = await _votesCollection.Find(filter).ToListAsync();

        return votes
            .Select(v => v["userId"].AsString)
            .ToList();
    }
}