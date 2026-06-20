using Microsoft.EntityFrameworkCore;
using CisApi.Src.Core.Domain.Entities;
using CisApi.Src.Infrastructure.MySQL.Mappings;

namespace CisApi.Src.Infrastructure.MySQL.Context;

/// <summary>
/// Represents the database context for the application.
/// </summary>
public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    // Represents tables in the database
    public DbSet<Topic> Topics { get; set; }
    public DbSet<Idea> Ideas { get; set; }
    public DbSet<Vote> Votes { get; set; }

    /// <summary>
    /// Configures the entity mappings using Fluent API.
    /// </summary>
    /// <param name="modelBuilder">Model builder provided by EF Core.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TopicConfiguration());
        modelBuilder.ApplyConfiguration(new IdeaConfiguration());
        modelBuilder.ApplyConfiguration(new VoteConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
