using CisApi.Src.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CisApi.Src.Infrastructure.MySQL.Mappings;

/// <summary>
/// Configures the database mapping for the Topic entity using EF Core Fluent API.
/// Defines table structure, constraints and database rules.
/// </summary>
public class TopicConfiguration : IEntityTypeConfiguration<Topic>
{
    public void Configure(EntityTypeBuilder<Topic> builder)
    {
        // Map entity to "topics" table
        builder.ToTable("topics");

        builder.HasKey(t => t.Id); // PK
        builder.Property(t => t.Id)
            .HasColumnName("id")
            .ValueGeneratedNever(); // Guid assigned manually

        builder.Property(t => t.Title)
            .IsRequired()
            .HasColumnName("title")
            .HasMaxLength(100);

        builder.Property(t => t.Description)
            .IsRequired()
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(t => t.CreatedAt)
            .HasColumnName("created_at")
            .HasPrecision(0) // no fractional seconds
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // FK to users table (shared responsibility)
        builder.Property(t => t.UserId)
            .IsRequired()
            .HasColumnName("user_id")
            .HasMaxLength(50);
    }
}