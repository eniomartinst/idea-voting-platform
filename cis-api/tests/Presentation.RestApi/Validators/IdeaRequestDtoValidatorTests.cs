using CisApi.Src.Presentation.RestApi.Dtos;
using CisApi.Src.Presentation.RestApi.Validators;
using FluentValidation.TestHelper;

namespace CisApi.Test.Presentation.RestApi.Validators;

public class IdeaRequestDtoValidatorTests
{
    private readonly IdeaRequestDtoValidator _dtoValidator = new();

    [Fact]
    public void Should_Have_Error_When_Content_Is_Null()
    {
        var model = new IdeaRequestDto
        {
            Content = null
        };

        var result = _dtoValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Content)
            .WithErrorMessage("Content cannot be empty");
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Empty()
    {
        var model = new IdeaRequestDto
        {
            Content = ""
        };

        var result = _dtoValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Have_Error_When_Content_Is_Whitespace()
    {
        var model = new IdeaRequestDto
        {
            Content = "   "
        };

        var result = _dtoValidator.TestValidate(model);

        result.ShouldHaveValidationErrorFor(x => x.Content);
    }

    [Fact]
    public void Should_Pass_When_Content_Is_Valid()
    {
        var model = new IdeaRequestDto
        {
            Content = "watch movies"
        };

        var result = _dtoValidator.TestValidate(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Content);
    }
}