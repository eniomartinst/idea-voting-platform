using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Presentation.RestApi.Dtos;

namespace CisApi.Src.Presentation.RestApi.Mappers;

/// <summary>
/// Handles mapping between Idea domain entity and API DTOs.
/// </summary>
public static class IdeaMapper
{
    /// <summary>
    /// Converts a request DTO into an Idea domain entity.
    /// </summary>
    public static Idea ToDomain(IdeaRequestDto dto, string topicId, ICurrentUser user)
    {
        return new Idea
        {
            Id = Guid.NewGuid().ToString(),
            Content = dto.Content,
            VotesCount = 0,
            CreatedAt = default,
            TopicId = topicId,
            UserId = user.Id
        };
    }

    /// <summary>
    /// Converts an Idea domain entity into a response DTO.
    /// </summary>
    public static IdeaResponseDto ToResponse(
        Idea idea,
        string topicTitle,
        UserReference? user,
        List<string> votedBy)
    {
        return new IdeaResponseDto
        {
            Id = idea.Id,
            Content = idea.Content,
            VotesCount = idea.VotesCount,
            VotedBy = votedBy,
            CreatedAt = idea.CreatedAt,

            Topic = new TopicIdeaDto(idea.TopicId, topicTitle),

            CreatedBy = user is null
                ? null
                : new UserDto(user.Id, user.Name, user.Login)
        };
    }
}