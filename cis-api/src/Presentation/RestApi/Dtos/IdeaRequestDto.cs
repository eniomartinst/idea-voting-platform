namespace CisApi.Src.Presentation.RestApi.Dtos;

/// <summary>
/// Request DTO for creating a new Idea.
/// </summary>
public class IdeaRequestDto
{
    public string Content { get; set; } = string.Empty;
}
