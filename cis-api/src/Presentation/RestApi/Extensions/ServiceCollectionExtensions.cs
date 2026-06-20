using CisApi.Src.Presentation.RestApi.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace CisApi.Src.Presentation.RestApi.Extensions;

/// <summary>
/// Extension methods to register Presentation layer services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Registers controllers and FluentValidation for request validation.
    /// </summary>
    public static IServiceCollection AddPresentationServices(this IServiceCollection services)
    {
        // Adds support for API controllers
        services.AddControllers();
        // Enables automatic validation using FluentValidation
        services.AddFluentValidationAutoValidation();
        // Registers all validators from the current assembly
        services.AddValidatorsFromAssemblyContaining<TopicRequestDtoValidator>();

        return services;
    }
}