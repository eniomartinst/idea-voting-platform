using CisApi.Src.Core.Application.Services;
using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Repositories;
using Moq;

namespace CisApi.Test.Core.Application.Services;

public class TopicCommandServiceTests
{
    [Fact]
    public async Task CreateTopicAsync_ShouldCallRepository()
    {
        var repoMock = new Mock<ITopicRepository>();
        repoMock.Setup(r => r.CreateTopicAsync(It.IsAny<Topic>()))
            .ReturnsAsync((Topic t) => t);

        var service = new TopicCommandService(repoMock.Object);

        var topic = new Topic { Title = "Title", Description = "Desc" };

        var result = await service.CreateTopicAsync(topic);

        repoMock.Verify(r => r.CreateTopicAsync(topic), Times.Once);
        Assert.Equal(topic.Title, result.Title);
    }
}