using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Core.Domain.Services;
using CisApi.Src.Core.Domain.ValueObjects;
using CisApi.Src.Presentation.RestApi.Dtos;
using CisApi.Src.Presentation.RestApi.Mappers;
using Moq;

namespace CisApi.Test.Presentation.RestApi.Mappers;

public class IdeaMapperTests
{
    [Fact]
    public void Should_Map_Dto_To_Domain_Correctly()
    {
        var dto = new IdeaRequestDto { Content = "watch movies" };

        var userMock = new Mock<ICurrentUser>();
        userMock.Setup(x => x.Id).Returns("user-1");

        var result = IdeaMapper.ToDomain(dto, "topic-1", userMock.Object);

        Assert.Equal("watch movies", result.Content);
        Assert.Equal("topic-1", result.TopicId);
        Assert.Equal("user-1", result.UserId);
    }

    [Fact]
    public void Should_Map_Domain_To_Response()
    {
        var idea = new Idea
        {
            Id = "1",
            Content = "watch movies",
            VotesCount = 0,
            TopicId = "topic-1",
            UserId = "user-1"
        };

        var topic = new Topic
        {
            Id = "topic-1",
            Title = "topic 1"
        };

        var user = new UserReference("user-1", "javier", "javier");
        var votedBy = new List<string> { "javier" };

        var result = IdeaMapper.ToResponse(idea, topic.Title, user, votedBy);

        Assert.Equal("watch movies", result.Content);
        Assert.Equal("topic 1", result.Topic.Title);
        Assert.Contains("javier", result.VotedBy);
    }

    [Fact]
    public void Should_Map_Domain_To_Response_When_User_Is_Null()
    {
        var idea = new Idea
        {
            Id = "1",
            Content = "watch movies",
            TopicId = "t1"
        };

        var votedBy = new List<string>();

        var result = IdeaMapper.ToResponse(idea, "Title", null, votedBy);

        Assert.Null(result.CreatedBy);
        Assert.Empty(result.VotedBy);
    }
}