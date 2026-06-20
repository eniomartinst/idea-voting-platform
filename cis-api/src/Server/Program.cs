using CisApi.Src.Infrastructure.IoC;
using CisApi.Src.Infrastructure.MySQL.Extensions;
using CisApi.Src.Infrastructure.MongoDB.Extensions;
using CisApi.Src.Presentation.RestApi.Middleware;

var builder = WebApplication.CreateBuilder(args);

// CIS API IoC Configuration
// Centralizes registration of application, auth, http and presentation layers
builder.Services.AddCisServices();

// Selects the persistence provider dynamically based on configuration
// Allows switching between MySQL and MongoDB without code changes
var provider = builder.Configuration["DatabaseConfig:Provider"]?.ToLowerInvariant() ?? "mongodb";
switch (provider)
{
    case "mongodb":
        // Registers MongoDB infrastructure (collections: topics, ideas, votes)
        builder.Services.AddMongoInfrastructureServices(builder.Configuration);
        break;

    case "mysql":
        // Registers MySQL infrastructure (tables: topics, ideas, votes)
        builder.Services.AddMySqlInfrastructureServices(builder.Configuration);
        break;
}

var app = builder.Build();

if (provider == "mysql")
{
    // Apply pending database migrations on startup using MySQL infrastructure extension
    DbMigrationExtensions.ApplyMigrations(app.Services);
}

// Global exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

// Auth middleware responsible for JWT validation and claim extraction.
// Sets HttpContext.User to be consumed by ICurrentUser abstraction.
app.UseMiddleware<AuthMiddleware>();

// Maps controller endpoints
app.MapControllers();

// Exposes API on Docker container port
app.Urls.Add("http://+:8005");

app.Run();
