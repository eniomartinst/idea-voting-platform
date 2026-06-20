namespace CisApi.Src.Core.Domain.Entities;

public class Vote
{
    public string IdeaId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}