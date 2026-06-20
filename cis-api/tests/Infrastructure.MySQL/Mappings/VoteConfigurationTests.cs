using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Infrastructure.MySQL.Mappings;
using Microsoft.EntityFrameworkCore;

namespace CisApi.Test.Infrastructure.MySQL.Mappings;

public class VoteConfigurationTests
{
    [Fact]
    public void Configure_Should_MapVoteToTable_Correctly()
    {
        // Arrange
        var modelBuilder = new ModelBuilder();
        var configuration = new VoteConfiguration();

        // Act
        modelBuilder.ApplyConfiguration(configuration);
        var entity = modelBuilder.Model.FindEntityType(typeof(Vote));

        // Assert
        Assert.NotNull(entity);
        Assert.Equal("votes", entity.GetTableName());

        // Test PK composite
        var pk = entity.FindPrimaryKey();
        Assert.NotNull(pk);
        Assert.Equal(2, pk.Properties.Count);

        // Test IdeaId
        var ideaId = entity.FindProperty(nameof(Vote.IdeaId));
        Assert.NotNull(ideaId);
        Assert.Equal("idea_id", ideaId!.GetColumnName());
        Assert.Equal(36, ideaId.GetMaxLength());
        Assert.False(ideaId.IsNullable);

        // Test UserId
        var userId = entity.FindProperty(nameof(Vote.UserId));
        Assert.NotNull(userId);
        Assert.Equal("user_id", userId!.GetColumnName());
        Assert.Equal(36, userId.GetMaxLength());

        // Test CreatedAt
        var createdAt = entity.FindProperty(nameof(Vote.CreatedAt));
        Assert.NotNull(createdAt);
        Assert.Equal("created_at", createdAt!.GetColumnName());
        Assert.Equal("CURRENT_TIMESTAMP", createdAt.GetDefaultValueSql());
    }
}