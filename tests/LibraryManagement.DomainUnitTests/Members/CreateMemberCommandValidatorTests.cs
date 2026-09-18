using FluentAssertions;
using LibraryManagement.Domain.Members;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Members;

public class CreateMemberCommandValidatorTests
{
    private readonly CreateMemberCommandValidator _validator = new();

    [Theory]
    [InlineData("Alice", "Standard")]
    [InlineData("Bob", "Student")]
    public void A_valid_command_passes_validation(string name, string profile)
    {
        var result = _validator.Validate(new CreateMemberCommand(name, profile));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData("", "Standard")]
    [InlineData("Alice", "")]
    [InlineData("Alice", "Unknown")]
    public void An_invalid_command_fails_validation(string name, string profile)
    {
        var result = _validator.Validate(new CreateMemberCommand(name, profile));

        result.IsValid.Should().BeFalse();
    }
}
