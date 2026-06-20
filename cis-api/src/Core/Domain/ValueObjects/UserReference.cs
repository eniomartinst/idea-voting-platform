namespace CisApi.Src.Core.Domain.ValueObjects;

/// <summary>
/// Represents a lightweight reference to a user within the domain.
/// 
/// This value object is used to decouple the domain from the Users service,
/// carrying only the necessary user data required for read operations.
/// </summary>
public record UserReference(string Id, string Name, string Login);
