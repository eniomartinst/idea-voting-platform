using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Infrastructure.MySQL.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Test.Infrastructure.MySQL.Mappings;

public class IdeaConfigurationTests
{
    [Fact]
    public void Configure_MapsIdeaToIdeasTable_WithCorrectProperties()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();
        var configuration = new IdeaConfiguration();

        // Act
        modelBuilder.ApplyConfiguration(configuration);
        var entityType = modelBuilder.Model.FindEntityType(typeof(Idea));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("ideas", entityType.GetTableName());

        // Validate Id
        var idProperty = entityType.FindProperty(nameof(Idea.Id));
        Assert.NotNull(idProperty);
        Assert.Equal("id", idProperty.GetColumnName());

        // Validate Content
        var contentProperty = entityType.FindProperty(nameof(Idea.Content));
        Assert.NotNull(contentProperty);
        Assert.Equal("content", contentProperty.GetColumnName());
        Assert.False(contentProperty.IsNullable);
        Assert.Equal(500, contentProperty.GetMaxLength());

        // Validate VotesCount
        var votesProperty = entityType.FindProperty(nameof(Idea.VotesCount));
        Assert.NotNull(votesProperty);
        Assert.Equal("votes_count", votesProperty.GetColumnName());
        Assert.Equal(0, votesProperty.GetDefaultValue());

        // Validate CreatedAt
        var createdAtProperty = entityType.FindProperty(nameof(Idea.CreatedAt));
        Assert.NotNull(createdAtProperty);
        Assert.Equal("created_at", createdAtProperty.GetColumnName());
        Assert.Equal("CURRENT_TIMESTAMP", createdAtProperty.GetDefaultValueSql());

        // Validate TopicId
        var topicIdProperty = entityType.FindProperty(nameof(Idea.TopicId));
        Assert.NotNull(topicIdProperty);
        Assert.Equal("topic_id", topicIdProperty.GetColumnName());
        Assert.False(topicIdProperty.IsNullable);

        // Validate UserId
        var userIdProperty = entityType.FindProperty(nameof(Idea.UserId));
        Assert.NotNull(userIdProperty);
        Assert.Equal("user_id", userIdProperty.GetColumnName());
        Assert.False(userIdProperty.IsNullable);
    }
}