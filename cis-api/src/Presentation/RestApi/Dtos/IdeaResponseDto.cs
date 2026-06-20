namespace CisApi.Src.Presentation.RestApi.Dtos;

/// <summary>
/// Response DTO representing an Idea.
/// </summary>
public class IdeaResponseDto
{
    public string Id { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int VotesCount { get; set; }

    public List<string> VotedBy { get; set; } = [];
    public DateTime CreatedAt { get; set; }

    public TopicIdeaDto Topic { get; set; } = null!;
    public UserDto? CreatedBy { get; set; }
}
