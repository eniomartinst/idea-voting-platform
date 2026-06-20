using CisApi.Src.Core.Domain.ValueObjects;

namespace CisApi.Src.Core.Domain.Services;

/// <summary>
/// Defines operations for retrieving user data from external sources.
/// </summary>
public interface IUserQueryService
{
    /// <summary>
    /// Gets a user by its identifier.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>
    /// A <see cref="UserReference"/> if found; otherwise, null.
    /// </returns>
    Task<UserReference?> GetByIdAsync(string userId);
}
