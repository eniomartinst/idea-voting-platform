using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace CisApi.Src.Presentation.RestApi.Middleware;

/// <summary>
/// Middleware responsible for extracting and validating JWT tokens from the Authorization header.
/// It parses the token payload and populates the HttpContext.User with the corresponding claims.
/// </summary>
public class AuthMiddleware
{

    private const string BearerPrefix = "Bearer ";
    private const string ClaimUserId = "userId";
    private const string ClaimLogin = "login";
    private const string ClaimSub = "sub";
    private const string ClaimName = "name";

    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuthMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the request pipeline.</param>
    public AuthMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Processes the incoming HTTP request, validating the JWT token and populating user claims.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var authHeader = context.Request.Headers["Authorization"].ToString();

        // Validate Authorization header format
        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith(BearerPrefix))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        // Extract token from header
        var token = authHeader[BearerPrefix.Length..].Trim();
        // Decode JWT payload
        var payload = DecodeJwtPayload(token);
        if (payload is null)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        // Required field validation
        if (!payload.TryGetValue(ClaimUserId, out var userIdObj))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        var userId = userIdObj?.ToString();
        if (string.IsNullOrWhiteSpace(userId))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        // Extract optional fields
        payload.TryGetValue(ClaimSub, out var loginObj);
        payload.TryGetValue(ClaimName, out var nameObj);

        var login = loginObj?.ToString() ?? "";
        var name = nameObj?.ToString() ?? "";

        // Build claims identity
        List<Claim> claims = [new(ClaimTypes.NameIdentifier, userId)];
        if (!string.IsNullOrWhiteSpace(login)) claims.Add(new Claim(ClaimLogin, login));
        if (!string.IsNullOrWhiteSpace(name)) claims.Add(new Claim(ClaimName, name));

        // Attach user to HttpContext
        context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, "Bearer"));
        // Continue pipeline
        await _next(context);
    }

    /// <summary>
    /// Decodes the payload section of a JWT token.
    /// </summary>
    /// <param name="token">The JWT token.</param>
    /// <returns>
    /// A dictionary representing the payload claims, or null if decoding fails.
    /// </returns>
    internal static Dictionary<string, object>? DecodeJwtPayload(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length < 2) return null;

            var payload = parts[1];
            var json = Base64UrlDecode(payload);
            return JsonSerializer.Deserialize<Dictionary<string, object>>(json);
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Decodes a Base64Url-encoded string into plain text.
    /// </summary>
    /// <param name="input">The Base64Url-encoded string.</param>
    /// <returns>The decoded string.</returns>
    internal static string Base64UrlDecode(string input)
    {
        var output = input.Replace('-', '+').Replace('_', '/');
        switch (output.Length % 4)
        {
            case 2: output += "=="; break;
            case 3: output += "="; break;
        }
        var bytes = Convert.FromBase64String(output);
        return System.Text.Encoding.UTF8.GetString(bytes);
    }
}