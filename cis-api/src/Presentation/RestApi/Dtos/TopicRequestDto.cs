namespace CisApi.Src.Presentation.RestApi.Dtos;

/// <summary>
/// DTO used to receive data when creating a new topic.
/// Represents the request body of POST /api/v1/topics.
/// </summary>
public class TopicRequestDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
