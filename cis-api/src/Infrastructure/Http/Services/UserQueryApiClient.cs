using System.Net.Http.Json;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Infrastructure.Http.Dtos;

namespace CisApi.Src.Infrastructure.Http.Services;

/// <summary>
/// Service responsible for retrieving user data from the Users API.
/// Acts as an adapter between the external HTTP contract and the domain model.
/// </summary>
public class UserQueryApiClient : IUserQueryService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserQueryApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">Configured HTTP client for the Users API.</param>
    public UserQueryApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Retrieves a user reference by its identifier from the Users API.
    /// </summary>
    /// <param name="userId">The user identifier.</param>
    /// <returns>
    /// A <see cref="UserReference"/> if found; otherwise, null.
    /// </returns>
    public async Task<UserReference?> GetByIdAsync(string userId)
    {
        var response = await _httpClient.GetAsync($"/api/v1/users/{userId}");
        if (!response.IsSuccessStatusCode) return null;

        var dto = await response.Content.ReadFromJsonAsync<UserResponseDto>();
        return dto == null ? null :
            // Map external DTO to domain user reference
            new UserReference(dto.Id, dto.Name, dto.Login);
    }
}
