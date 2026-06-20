using CisApi.Src.Presentation.RestApi.Dtos;
using FluentValidation;

namespace CisApi.Src.Presentation.RestApi.Validators;

/// <summary>
/// Validator for TopicRequestDto.
/// Ensures that incoming requests contain valid data.
/// </summary>
public class TopicRequestDtoValidator : AbstractValidator<TopicRequestDto>
{
    public TopicRequestDtoValidator()
    {
        RuleFor(command => command.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(100);

        RuleFor(command => command.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(500);
    }
}