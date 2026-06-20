using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Presentation.RestApi.Controllers;
using CisApi.Src.Presentation.RestApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CisApi.Test.Presentation.RestApi.Controllers;

public class TopicQueryControllerTests
{
    [Fact]
    public async Task GetTopicsAsync_ShouldReturnOk_WithTopics()
    {
        var topics = new List<Topic>
        {
            new Topic { Id = "1", Title = "T1", UserId = "U1" },
            new Topic { Id = "2", Title = "T2", UserId = "U1" }
        };

        var mockService = new Mock<ITopicQueryService>();
        mockService.Setup(s => s.GetTopicsAsync()).ReturnsAsync(topics);

        var mockUserService = new Mock<IUserQueryService>();
        mockUserService.Setup(u => u.GetByIdAsync("U1")).ReturnsAsync(new UserReference("U1", "User", "login"));

        var mockIdeaRepo = new Mock<IIdeaRepository>();
        mockIdeaRepo.Setup(r => r.GetIdeasByTopicIdAsync(It.IsAny<string>())).ReturnsAsync(new List<Idea>());

        var controller = new TopicQueryController(mockService.Object, mockUserService.Object, mockIdeaRepo.Object);

        var result = await controller.GetTopicsAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var data = Assert.IsAssignableFrom<IEnumerable<TopicResponseDto>>(okResult.Value);

        Assert.Equal(2, data.Count());
    }

    [Fact]
    public async Task GetTopicsAsync_ShouldReturnEmptyList_WhenNoTopics()
    {
        var mockService = new Mock<ITopicQueryService>();
        mockService.Setup(s => s.GetTopicsAsync()).ReturnsAsync(new List<Topic>());

        var mockUserService = new Mock<IUserQueryService>();
        var mockIdeaRepo = new Mock<IIdeaRepository>();

        var controller = new TopicQueryController(mockService.Object, mockUserService.Object, mockIdeaRepo.Object);

        var result = await controller.GetTopicsAsync();

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var data = Assert.IsAssignableFrom<IEnumerable<TopicResponseDto>>(okResult.Value);

        Assert.Empty(data);
    }
}