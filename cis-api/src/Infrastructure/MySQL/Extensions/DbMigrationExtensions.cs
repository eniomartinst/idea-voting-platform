using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CisApi.Src.Infrastructure.MySQL.Context;

namespace CisApi.Src.Infrastructure.MySQL.Extensions;

/// <summary>
/// Provides extension methods to apply database migrations for the MySQL infrastructure.
/// </summary>
public static class DbMigrationExtensions
{
    /// <summary>
    /// Applies pending EF Core migrations on application startup.
    /// </summary>
    /// <param name="services">
    /// The application's <see cref="IServiceProvider"/> used to create a scoped
    /// service instance for resolving <see cref="MyDbContext"/>.
    /// </param>
    /// <remarks>
    /// It includes a retry mechanism to handle transient failures.
    /// </remarks>
    public static void ApplyMigrations(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MyDbContext>();

        const int retries = 3;
        const int delay = 2000; // 2 seconds

        for (var i = 0; i < retries; i++)
        {
            try
            {
                dbContext.Database.Migrate();
                break; // ok
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Migration failed ({i + 1}/{retries}): {ex.Message}");
                Thread.Sleep(delay);
            }
        }
    }
}