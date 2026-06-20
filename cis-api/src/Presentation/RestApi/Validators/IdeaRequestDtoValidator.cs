using CisApi.Src.Presentation.RestApi.Dtos;

namespace CisApi.Src.Presentation.RestApi.Validators;

using FluentValidation;

/// <summary>
/// Validator for IdeaRequestDto.
/// Ensures that incoming requests contain valid content.
/// </summary>
public class IdeaRequestDtoValidator : AbstractValidator<IdeaRequestDto>
{
    public IdeaRequestDtoValidator()
    {
        RuleFor(command => command.Content)
            .NotEmpty()
            .WithMessage("Content cannot be empty");
    }
}
