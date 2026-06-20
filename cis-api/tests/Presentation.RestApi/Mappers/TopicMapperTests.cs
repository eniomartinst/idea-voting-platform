using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Presentation.RestApi.Dtos;
using CisApi.Src.Presentation.RestApi.Mappers;
using Moq;

namespace CisApi.Test.Presentation.RestApi.Mappers;

public class TopicMapperTests
{
    [Fact]
    public void ToDomain_ShouldMapDtoToEntity()
    {
        // Arrange
        var dto = new TopicRequestDto
        {
            Title = "Sample",
            Description = "Sample description"
        };

        var userMock = new Mock<ICurrentUser>();
        userMock.Setup(x => x.Id).Returns("user-1");

        // Act
        var topic = TopicMapper.ToDomain(dto, userMock.Object);

        // Assert
        Assert.NotNull(topic.Id);
        Assert.Equal(dto.Title, topic.Title);
        Assert.Equal(dto.Description, topic.Description);
        Assert.Equal("user-1", topic.UserId);
    }

    [Fact]
    public void ToResponse_ShouldMapEntityToDto()
    {
        // Arrange
        var entity = new Topic
        {
            Id = "123",
            Title = "Sample",
            Description = "Sample description",
            CreatedAt = DateTime.UtcNow,
            UserId = "user-1"
        };

        var user = new UserReference(
            "user-123",
            "John Doe",
            "johndoe"
        );

        var dto = TopicMapper.ToResponse(entity, user, 7);

        // Assert
        Assert.Equal(entity.Id, dto.Id);
        Assert.Equal(entity.Title, dto.Title);
        Assert.Equal(7, dto.TotalIdeasCount);

        Assert.NotNull(dto.CreatedBy);
        Assert.Equal("user-123", dto.CreatedBy.Id);
    }

    [Fact]
    public void ToResponse_ShouldMapEntityToDto_WhenUserIsNull()
    {
        var entity = new Topic { Id = "123" };
        var dto = TopicMapper.ToResponse(entity, null, 7);
        Assert.Null(dto.CreatedBy);
    }
}