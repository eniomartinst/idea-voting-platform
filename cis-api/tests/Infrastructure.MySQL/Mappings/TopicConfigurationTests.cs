using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Infrastructure.MySQL.Mappings;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CisApi.Test.Infrastructure.MySQL.Mappings;

public class TopicConfigurationTests
{
    [Fact]
    public void Configure_MapsTopicToTopicsTable_WithCorrectProperties()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();
        var configuration = new TopicConfiguration();

        // Act
        modelBuilder.ApplyConfiguration(configuration);
        var entityType = modelBuilder.Model.FindEntityType(typeof(Topic));

        // Assert
        Assert.NotNull(entityType);
        Assert.Equal("topics", entityType.GetTableName());

        // Test Primary Key
        var idProperty = entityType.FindProperty(nameof(Topic.Id));
        Assert.NotNull(idProperty);
        Assert.Equal("id", idProperty.GetColumnName());
        Assert.True(idProperty.IsPrimaryKey());

        // Test Title
        var titleProperty = entityType.FindProperty(nameof(Topic.Title));
        Assert.NotNull(titleProperty);
        Assert.Equal("title", titleProperty.GetColumnName());
        Assert.False(titleProperty.IsNullable);
        Assert.Equal(100, titleProperty.GetMaxLength());

        // Test Description
        var descriptionProperty = entityType.FindProperty(nameof(Topic.Description));
        Assert.NotNull(descriptionProperty);
        Assert.Equal("description", descriptionProperty.GetColumnName());
        Assert.False(descriptionProperty.IsNullable);
        Assert.Equal(500, descriptionProperty.GetMaxLength());

        // Test CreatedAt
        var createdAtProperty = entityType.FindProperty(nameof(Topic.CreatedAt));
        Assert.NotNull(createdAtProperty);
        Assert.Equal("created_at", createdAtProperty.GetColumnName());
        Assert.Equal("CURRENT_TIMESTAMP", createdAtProperty.GetDefaultValueSql());

        // Test UserId
        var userIdProperty = entityType.FindProperty(nameof(Topic.UserId));
        Assert.NotNull(userIdProperty);
        Assert.Equal("user_id", userIdProperty.GetColumnName());
        Assert.False(userIdProperty.IsNullable);
        Assert.Equal(50, userIdProperty.GetMaxLength());
    }
}
