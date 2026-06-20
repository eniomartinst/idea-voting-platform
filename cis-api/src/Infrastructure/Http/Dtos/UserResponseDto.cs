namespace CisApi.Src.Infrastructure.Http.Dtos;

/// <summary>
/// Represents the user response returned by the Users API.
/// </summary>
public record UserResponseDto(string Id, string Name, string Login);
