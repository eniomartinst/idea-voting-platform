using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.Exceptions;

namespace CisApi.Src.Core.Application.Services;

/// <summary>
/// Application service responsible for handling command operations related to <see cref="Idea"/>.
/// Coordinates domain validation and persistence using repositories.
/// </summary>
public class IdeaCommandService : IIdeaCommandService
{
    private readonly IIdeaRepository _ideaRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly IVoteRepository _voteRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="IdeaCommandService"/> class.
    /// </summary>
    /// <param name="ideaRepository">Repository for idea persistence operations.</param>
    /// <param name="topicRepository">Repository for topic validation and retrieval.</param>
    /// <param name="voteRepository">Repository for voting operations.</param>
    public IdeaCommandService(
        IIdeaRepository ideaRepository,
        ITopicRepository topicRepository,
        IVoteRepository voteRepository)
    {
        _ideaRepository = ideaRepository;
        _topicRepository = topicRepository;
        _voteRepository = voteRepository;
    }

    /// <summary>
    /// Creates a new idea associated with a topic.
    /// Validates that the topic exists before persisting the idea.
    /// </summary>
    /// <param name="idea">The idea entity to be created.</param>
    /// <returns>The persisted <see cref="Idea"/> entity.</returns>
    /// <exception cref="KeyNotFoundException">Thrown when the topic does not exist.</exception>
    public async Task<Idea> CreateIdeaAsync(Idea idea)
    {
        var topic = await _topicRepository.GetTopicAsync(idea.TopicId);
        if (topic is null) throw new NotFoundException("Topic", idea.TopicId);

        return await _ideaRepository.CreateIdeaAsync(idea);
    }

    public async Task<Idea> VoteIdeaAsync(string ideaId, ICurrentUser currentUser)
    {
        var idea = await _ideaRepository.GetIdeaByIdAsync(ideaId);
        if (idea is null) throw new NotFoundException("Idea", ideaId);

        if (idea.UserId == currentUser.Id)
            throw new OperationNotAllowedException("Owner cannot vote on their own idea.");

        var alreadyVoted = await _voteRepository.ExistsAsync(ideaId, currentUser.Id);
        if (alreadyVoted)
            throw new OperationNotAllowedException("User already voted on this idea.");

        await _voteRepository.AddAsync(ideaId, currentUser.Id);

        idea.VotesCount = await _voteRepository.CountByIdeaIdAsync(ideaId);
        return await _ideaRepository.UpdateIdeaAsync(idea);
    }

    public async Task<Idea> UnvoteIdeaAsync(string ideaId, ICurrentUser currentUser)
    {
        var idea = await _ideaRepository.GetIdeaByIdAsync(ideaId);
        if (idea is null) throw new NotFoundException("Idea", ideaId);

        var alreadyVoted = await _voteRepository.ExistsAsync(ideaId, currentUser.Id);
        if (!alreadyVoted)
            throw new OperationNotAllowedException("User has not voted on this idea.");

        await _voteRepository.RemoveAsync(ideaId, currentUser.Id);

        idea.VotesCount = await _voteRepository.CountByIdeaIdAsync(ideaId);
        return await _ideaRepository.UpdateIdeaAsync(idea);
    }
}