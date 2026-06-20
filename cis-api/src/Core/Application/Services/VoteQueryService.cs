using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;

namespace CisApi.Src.Core.Application.Services;

public class VoteQueryService : IVoteQueryService
{
    private readonly IVoteRepository _voteRepository;
    private readonly IUserQueryService _userQueryService;

    public VoteQueryService(IVoteRepository voteRepository, IUserQueryService userQueryService)
    {
        _voteRepository = voteRepository ?? throw new ArgumentNullException(nameof(voteRepository));
        _userQueryService = userQueryService ?? throw new ArgumentNullException(nameof(userQueryService));
    }

    public async Task<List<string>> GetLoginsByIdeaIdAsync(string ideaId)
    {
        var userIds = await _voteRepository.GetUserIdsByIdeaIdAsync(ideaId);

        var logins = new List<string>();

        foreach (var userId in userIds)
        {
            var user = await _userQueryService.GetByIdAsync(userId);
            if (user is not null) logins.Add(user.Login);
        }

        return logins;
    }
}