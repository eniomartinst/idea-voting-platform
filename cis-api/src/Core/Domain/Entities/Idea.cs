namespace CisApi.Src.Core.Domain.Entities;

/// <summary>
/// Represents an Idea in the domain layer.
/// Contains user-generated content linked to a Topic.
/// </summary>
public class Idea
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int VotesCount { get; set; }
    public DateTime CreatedAt { get; set; }

    public string TopicId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}
