using CisApi.Src.Presentation.RestApi.Middleware;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Text;

namespace CisApi.Test.Presentation.RestApi.Middleware;

public class AuthMiddlewareTests
{
    private readonly Mock<RequestDelegate> _nextMock;
    private readonly AuthMiddleware _middleware;

    public AuthMiddlewareTests()
    {
        _nextMock = new Mock<RequestDelegate>();
        _middleware = new AuthMiddleware(_nextMock.Object);
    }

    [Fact]
    public async Task InvokeAsync_MissingAuthorizationHeader_Returns401()
    {
        var context = new DefaultHttpContext();

        await _middleware.InvokeAsync(context);

        Assert.Equal(401, context.Response.StatusCode);
        _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_MalformedHeader_Returns401()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "InvalidFormat";

        await _middleware.InvokeAsync(context);

        Assert.Equal(401, context.Response.StatusCode);
        _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_ValidToken_SetsUserAndCallsNext()
    {
        var context = new DefaultHttpContext();

        // {"userId":"123","sub":"testuser","name":"John"}
        var payload = Convert.ToBase64String(
            "{\"userId\":\"123\",\"sub\":\"testuser\",\"name\":\"John\"}"u8.ToArray()
        );

        var token = $"header.{payload}.signature";
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await _middleware.InvokeAsync(context);

        Assert.NotNull(context.User.Identity);
        Assert.True(context.User.Identity.IsAuthenticated);

        Assert.Equal("123", context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
        Assert.Equal("testuser", context.User.FindFirst("login")?.Value);
        Assert.Equal("John", context.User.FindFirst("name")?.Value);

        _nextMock.Verify(next => next(context), Times.Once);
    }

    [Fact]
    public async Task InvokeAsync_MissingUserId_Returns403()
    {
        var context = new DefaultHttpContext();

        // without userId ? should return 403 status code
        var payload = Convert.ToBase64String(
            Encoding.UTF8.GetBytes("{\"sub\":\"testuser\"}")
        );

        var token = $"header.{payload}.signature";
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await _middleware.InvokeAsync(context);

        Assert.Equal(403, context.Response.StatusCode);
        _nextMock.Verify(next => next(It.IsAny<HttpContext>()), Times.Never);
    }

    [Fact]
    public void DecodeJwtPayload_ValidPayload_ReturnsDictionary()
    {
        var payload = Convert.ToBase64String(
            "{\"userId\":\"123\"}"u8.ToArray()
        );

        var token = $"header.{payload}.signature";

        var result = AuthMiddleware.DecodeJwtPayload(token);

        Assert.NotNull(result);
        Assert.Equal("123", result["userId"].ToString());
    }

    [Fact]
    public void DecodeJwtPayload_InvalidBase64_ReturnsNull()
    {
        var token = "header.invalid_base64.signature";

        var result = AuthMiddleware.DecodeJwtPayload(token);

        Assert.Null(result);
    }

    [Fact]
    public async Task InvokeAsync_InvalidJwtPayload_Returns403()
    {
        var context = new DefaultHttpContext();
        context.Request.Headers["Authorization"] = "Bearer header.badpayload.signature";

        await _middleware.InvokeAsync(context);

        Assert.Equal(403, context.Response.StatusCode);
        _nextMock.Verify(x => x(It.IsAny<HttpContext>()), Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_EmptyUserId_Returns403()
    {
        var context = new DefaultHttpContext();

        var payload = Convert.ToBase64String("{\"userId\":\"\"}"u8.ToArray());

        var token = $"header.{payload}.signature";
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await _middleware.InvokeAsync(context);

        Assert.Equal(403, context.Response.StatusCode);
        _nextMock.Verify(x => x(It.IsAny<HttpContext>()), Times.Never);
    }

    [Fact]
    public async Task InvokeAsync_NullPayload_Returns403()
    {
        var context = new DefaultHttpContext();

        var token = "header..signature"; // empty payload
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await _middleware.InvokeAsync(context);

        Assert.Equal(403, context.Response.StatusCode);
    }

    [Fact]
    public async Task InvokeAsync_ValidToken_WithoutOptionalClaims_SetsUserAndCallsNext()
    {
        var context = new DefaultHttpContext();

        var payload = Convert.ToBase64String(
            System.Text.Encoding.UTF8.GetBytes("{\"userId\":\"777\"}")
        );

        var token = $"header.{payload}.signature";
        context.Request.Headers["Authorization"] = $"Bearer {token}";

        await _middleware.InvokeAsync(context);

        Assert.NotNull(context.User.Identity);
        Assert.True(context.User.Identity.IsAuthenticated);

        Assert.Equal("777", context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);

        Assert.Null(context.User.FindFirst("login"));
        Assert.Null(context.User.FindFirst("name"));

        _nextMock.Verify(next => next(context), Times.Once);
    }

    [Theory]
    [InlineData("YQ", "a")]       // Rest 2: test padding "=="
    [InlineData("YWI", "ab")]     // Rest 3: test padding "="
    [InlineData("YWJj", "abc")]   // Rest 0: not padding
    public void Base64UrlDecode_HandlesPaddingCorrectly(string input, string expected)
    {
        var result = AuthMiddleware.Base64UrlDecode(input);
        Assert.Equal(expected, result);
    }

    [Fact]
    public void DecodeJwtPayload_LessThanTwoParts_ReturnsNull()
    {
        var result = AuthMiddleware.DecodeJwtPayload("invalid_token_without_dots");
        Assert.Null(result);
    }
}