using CisApi.Src.Core.Application.Services;
using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using Moq;

namespace CisApi.Test.Core.Application.Services;

public class TopicQueryServiceTests
{
    [Fact]
    public async Task GetTopicsAsync_ShouldCallRepository()
    {
        var mockRepo = new Mock<ITopicRepository>();

        mockRepo.Setup(r => r.GetTopicsAsync())
            .ReturnsAsync(new List<Topic>());

        var service = new TopicQueryService(mockRepo.Object);

        var result = await service.GetTopicsAsync();

        mockRepo.Verify(r => r.GetTopicsAsync(), Times.Once);
    }
}