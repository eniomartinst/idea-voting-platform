namespace CisApi.Src.Core.Domain.Entities;

/// <summary>
/// Represents the Topic entity in the domain layer.
/// This model contains business data and relationships.
/// </summary>
public class Topic
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string UserId { get; set; } = string.Empty;

    public List<Idea> Ideas { get; set; } = [];
}
