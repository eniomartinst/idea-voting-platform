using CisApi.Src.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CisApi.Src.Infrastructure.MySQL.Mappings;

/// <summary>
/// Configures the database mapping for the Idea entity using EF Core Fluent API.
/// Defines table structure, constraints and database rules.
/// </summary>
public class IdeaConfiguration : IEntityTypeConfiguration<Idea>
{
    public void Configure(EntityTypeBuilder<Idea> builder)
    {
        // Map entity to "ideas" table
        builder.ToTable("ideas");

        // Primary Key
        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(i => i.Content)
            .IsRequired()
            .HasColumnName("content")
            .HasMaxLength(500);

        builder.Property(i => i.VotesCount)
            .HasColumnName("votes_count")
            .HasDefaultValue(0);

        builder.Property(i => i.CreatedAt)
            .HasColumnName("created_at")
            .HasPrecision(0)
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); // set by MySQL

        // FK to topics table
        builder.Property(i => i.TopicId)
            .IsRequired()
            .HasColumnName("topic_id")
            .HasMaxLength(50);

        // FK to users table
        builder.Property(i => i.UserId)
            .IsRequired()
            .HasColumnName("user_id")
            .HasMaxLength(50);
    }
}