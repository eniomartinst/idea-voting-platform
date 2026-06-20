using System.Net;
using CisApi.Src.Core.Domain.Exceptions;
using CisApi.Src.Presentation.RestApi.Middleware;
using Microsoft.AspNetCore.Http;

namespace CisApi.Test.Presentation.RestApi.Middleware;

public class ExceptionMiddlewareTests
{
    [Fact]
    public async Task InvokeAsync_ShouldHandleNotFoundException()
    {
        // Arrange
        var middleware = new ExceptionMiddleware((innerContext) =>
        {
            throw new NotFoundException("Topic", "123");
        });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.NotFound, context.Response.StatusCode);
        Assert.Equal("application/json", context.Response.ContentType);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        var responseBody = await reader.ReadToEndAsync();

        Assert.Contains("Topic with id 123 was not found.", responseBody);
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleGenericDomainException()
    {
        // Arrange
        var middleware = new ExceptionMiddleware((innerContext) =>
        {
            throw new OperationNotAllowedException("Action not permitted.");
        });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, context.Response.StatusCode);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        Assert.Contains("Action not permitted.", await reader.ReadToEndAsync());
    }

    [Fact]
    public async Task InvokeAsync_ShouldHandleGenericException()
    {
        // Arrange
        var middleware = new ExceptionMiddleware((innerContext) =>
        {
            throw new Exception("Unexpected error");
        });

        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal((int)HttpStatusCode.InternalServerError, context.Response.StatusCode);

        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(context.Response.Body);
        Assert.Contains("An internal error occurred.", await reader.ReadToEndAsync());
    }

    [Fact]
    public async Task InvokeAsync_ShouldCallNext_WhenNoException()
    {
        // Arrange
        var middleware = new ExceptionMiddleware(async (innerContext) =>
        {
            innerContext.Response.StatusCode = 200;
            await Task.CompletedTask;
        });

        var context = new DefaultHttpContext();

        // Act
        await middleware.InvokeAsync(context);

        // Assert
        Assert.Equal(200, context.Response.StatusCode);
    }
}