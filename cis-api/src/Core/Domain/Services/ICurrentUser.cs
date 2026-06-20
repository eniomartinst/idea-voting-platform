namespace CisApi.Src.Core.Domain.Services;

/// <summary>
/// Provides information about the currently authenticated user.
/// Used for auditing and ownership tracking in domain operations.
/// </summary>
public interface ICurrentUser
{
    string Id { get; }
    string Name { get; }
    string Login { get; }
}
