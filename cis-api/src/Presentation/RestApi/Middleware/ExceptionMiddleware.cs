using System.Net;
using System.Text.Json;
using CisApi.Src.Core.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace CisApi.Src.Presentation.RestApi.Middleware;

/// <summary>
/// Global exception handler middleware to catch domain exceptions and map them to HTTP responses.
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (DomainException ex)
        {
            await HandleDomainExceptionAsync(context, ex);
        }
        catch (Exception ex)
        {
            await HandleGenericExceptionAsync(context, ex);
        }
    }

    private static Task HandleDomainExceptionAsync(HttpContext context, DomainException ex)
    {
        var code = HttpStatusCode.BadRequest;
        string? resource = null;
        string? resourceId = null;

        if (ex is NotFoundException nfe)
        {
            code = HttpStatusCode.NotFound;
            resource = nfe.Resource;
            resourceId = nfe.ResourceId;
        }

        var result = JsonSerializer.Serialize(new
        {
            message = ex.Message,
            resource = resource,
            id = resourceId
        });

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(result);
    }

    private static Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
    {
        var result = JsonSerializer.Serialize(new { message = "An internal error occurred." });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        return context.Response.WriteAsync(result);
    }
}