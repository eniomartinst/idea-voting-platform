using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Presentation.RestApi.Controllers;
using CisApi.Src.Presentation.RestApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CisApi.Test.Presentation.RestApi.Controllers;

public class TopicCommandControllerTests
{
    [Fact]
    public async Task CreateTopicAsync_ReturnsCreatedResult_WithUserAndCount()
    {
        var serviceMock = new Mock<ITopicCommandService>();
        var currentUserMock = new Mock<ICurrentUser>();
        var userQueryMock = new Mock<IUserQueryService>();

        var topic = new Topic { Id = "t1", Title = "Test", Description = "Test desc", UserId = "u1" };

        serviceMock.Setup(s => s.CreateTopicAsync(It.IsAny<Topic>())).ReturnsAsync(topic);
        currentUserMock.Setup(x => x.Id).Returns("u1");
        userQueryMock.Setup(x => x.GetByIdAsync("u1")).ReturnsAsync(new UserReference("u1", "User 1", "user1"));

        var controller = new TopicCommandController(serviceMock.Object, currentUserMock.Object, userQueryMock.Object);

        var dto = new TopicRequestDto { Title = "Test", Description = "Test desc" };
        var result = await controller.CreateTopicAsync(dto);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(201, objectResult.StatusCode);

        var response = Assert.IsType<TopicResponseDto>(objectResult.Value);
        Assert.Equal(dto.Title, response.Title);
        Assert.Equal(0, response.TotalIdeasCount);
        Assert.NotNull(response.CreatedBy);
        Assert.Equal("u1", response.CreatedBy.Id);
    }

    [Fact]
    public async Task CreateTopicAsync_ShouldReturnNullUser_WhenUserApiFails()
    {
        var serviceMock = new Mock<ITopicCommandService>();
        var currentUserMock = new Mock<ICurrentUser>();
        var userQueryMock = new Mock<IUserQueryService>();

        var topic = new Topic { Id = "t1", Title = "Test", Description = "Test desc", UserId = "u1" };

        serviceMock.Setup(s => s.CreateTopicAsync(It.IsAny<Topic>())).ReturnsAsync(topic);
        currentUserMock.Setup(x => x.Id).Returns("u1");
        userQueryMock.Setup(x => x.GetByIdAsync("u1")).ReturnsAsync((UserReference?)null);

        var controller = new TopicCommandController(serviceMock.Object, currentUserMock.Object, userQueryMock.Object);

        var dto = new TopicRequestDto { Title = "Test", Description = "Test desc" };
        var result = await controller.CreateTopicAsync(dto);

        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        var response = Assert.IsType<TopicResponseDto>(objectResult.Value);

        Assert.Null(response.CreatedBy);
        Assert.Equal(0, response.TotalIdeasCount);
    }
}