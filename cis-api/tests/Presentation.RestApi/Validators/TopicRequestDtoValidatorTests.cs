using CisApi.Src.Presentation.RestApi.Dtos;
using CisApi.Src.Presentation.RestApi.Validators;
using FluentValidation.TestHelper;

namespace CisApi.Test.Presentation.RestApi.Validators;

public class TopicRequestDtoValidatorTests
{
    private readonly TopicRequestDtoValidator _validator = new();

    [Fact]
    public void Should_HaveError_WhenTitleIsEmpty()
    {
        var dto = new TopicRequestDto { Title = "", Description = "Valid description" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_HaveError_WhenDescriptionIsEmpty()
    {
        var dto = new TopicRequestDto { Title = "Valid Title", Description = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_NotHaveError_WhenDtoIsValid()
    {
        var dto = new TopicRequestDto { Title = "Title", Description = "Description" };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_HaveError_WhenTitleExceedsMaximumLength()
    {
        // Arrange
        var dto = new TopicRequestDto
        {
            Title = new string('A', 101), // Cria uma string com 101 caracteres
            Description = "Valid description"
        };

        // Act
        var result = _validator.TestValidate(dto);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Title);
    }

    [Fact]
    public void Should_HaveError_WhenTitleAndDescriptionExceedLimits()
    {
        var dto = new TopicRequestDto
        {
            Title = new string('A', 101),
            Description = new string('A', 501)
        };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_NotHaveError_WhenDataIsWithinLimits()
    {
        var dto = new TopicRequestDto { Title = new string('A', 50), Description = new string('B', 100) };
        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Title);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}