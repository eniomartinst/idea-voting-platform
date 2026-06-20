namespace CisApi.Src.Presentation.RestApi.Dtos;

/// <summary>
/// DTO returned after successfully creating a topic.
/// Represents the response of POST /api/v1/topics.
/// </summary>
public class TopicResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TotalIdeasCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public UserDto? CreatedBy { get; set; }
}
