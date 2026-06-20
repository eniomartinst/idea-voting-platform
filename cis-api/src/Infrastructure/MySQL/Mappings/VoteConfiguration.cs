using CisApi.Src.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CisApi.Src.Infrastructure.MySQL.Mappings;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.ToTable("votes");

        // Composite PK
        builder.HasKey(v => new { v.IdeaId, v.UserId });

        builder.Property(v => v.IdeaId)
            .HasColumnName("idea_id")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(v => v.UserId)
            .HasColumnName("user_id")
            .HasMaxLength(36)
            .IsRequired();

        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at")
            .HasPrecision(0)
            .HasDefaultValueSql("CURRENT_TIMESTAMP"); // set by MySQL

        // FK → ideas
        builder.HasOne<Idea>()
            .WithMany()
            .HasForeignKey(v => v.IdeaId)
            .OnDelete(DeleteBehavior.Cascade);

        // FK → users
        builder.HasIndex(v => v.UserId);
    }
}