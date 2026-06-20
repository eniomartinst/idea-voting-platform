using CisApi.Src.Core.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CisApi.Src.Infrastructure.Auth.Extension;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddHttpContextAccessor();

        return services;
    }
}