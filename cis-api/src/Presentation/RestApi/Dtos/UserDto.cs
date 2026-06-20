namespace CisApi.Src.Presentation.RestApi.Dtos;

/// <summary>
/// Represents user information exposed in API responses.
/// </summary>
public record UserDto(string Id, string Name, string Login);
