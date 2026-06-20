using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Presentation.RestApi.Dtos;

namespace CisApi.Src.Presentation.RestApi.Mappers;

/// <summary>
/// Handles mapping between Topic domain entity and API DTOs.
/// Keeps mapping logic centralized and avoids duplication in controllers/services.
/// </summary>
public static class TopicMapper
{
    /// <summary>
    /// Converts a request DTO into a Topic domain entity.
    /// The authenticated user is used only to set the foreign key (UserId).
    /// </summary>
    public static Topic ToDomain(TopicRequestDto dto, ICurrentUser user)
    {
        return new Topic
        {
            Id = Guid.NewGuid().ToString(),
            Title = dto.Title,
            Description = dto.Description,

            // FK to Users table (shared database responsibility)
            UserId = user.Id
        };
    }

    /// <summary>
    /// Converts a Topic domain entity into a response DTO.
    /// User information is resolved externally and passed as a value object.
    /// </summary>
    public static TopicResponseDto ToResponse(Topic topic, UserReference? user, int totalIdeasCount)
    {
        return new TopicResponseDto
        {
            Id = topic.Id,
            Title = topic.Title,
            Description = topic.Description,
            TotalIdeasCount = totalIdeasCount,
            CreatedAt = topic.CreatedAt,

            // Maps external user reference into API response contract
            CreatedBy = user is null
                ? null
                : new UserDto(user.Id, user.Name, user.Login)
        };
    }
}