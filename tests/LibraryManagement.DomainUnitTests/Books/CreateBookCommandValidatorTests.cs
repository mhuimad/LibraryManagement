using FluentAssertions;
using LibraryManagement.Domain.Books;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Books;

public class CreateBookCommandValidatorTests
{
    private readonly CreateBookCommandValidator _validator = new();

    [Fact]
    public void A_valid_command_passes_validation()
    {
        var result = _validator.Validate(new CreateBookCommand("Clean Code", "Robert C. Martin", 3));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Robert C. Martin", 3)]
    [InlineData("Clean Code", "", 3)]
    [InlineData("Clean Code", "Robert C. Martin", 0)]
    public void An_invalid_command_fails_validation(string title, string author, int totalCopies)
    {
        var result = _validator.Validate(new CreateBookCommand(title, author, totalCopies));

        result.IsValid.Should().BeFalse();
    }
}
