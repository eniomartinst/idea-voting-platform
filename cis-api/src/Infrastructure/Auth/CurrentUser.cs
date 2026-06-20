using System.Security.Claims;
using CisApi.Src.Core.Domain.Services;
using Microsoft.AspNetCore.Http;

namespace CisApi.Src.Infrastructure.Auth;

public class CurrentUser : ICurrentUser
{
    private const string ClaimLogin = "login";
    private const string ClaimName = "name";

    private readonly IHttpContextAccessor _context;

    public CurrentUser(IHttpContextAccessor context)
    {
        _context = context;
    }

    public string Id =>
        _context.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "";

    public string Name =>
        _context.HttpContext?.User.FindFirst(ClaimName)?.Value ?? "";

    public string Login =>
        _context.HttpContext?.User.FindFirst(ClaimLogin)?.Value ?? "";
}