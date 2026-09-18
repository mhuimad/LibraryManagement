using FluentAssertions;
using LibraryManagement.Domain.Members;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Members;

public class MemberProfileTests
{
    [Fact]
    public void Standard_profile_allows_three_simultaneous_loans_for_three_weeks()
    {
        var profile = MemberProfile.Standard;

        profile.Name.Should().Be("Standard");
        profile.MaxSimultaneousLoans.Should().Be(3);
        profile.LoanDurationInWeeks.Should().Be(3);
    }

    [Fact]
    public void Student_profile_allows_five_simultaneous_loans_for_four_weeks()
    {
        var profile = MemberProfile.Student;

        profile.Name.Should().Be("Student");
        profile.MaxSimultaneousLoans.Should().Be(5);
        profile.LoanDurationInWeeks.Should().Be(4);
    }

    [Theory]
    [InlineData("Standard")]
    [InlineData("Student")]
    public void FromName_returns_the_matching_profile(string name)
    {
        var profile = MemberProfile.FromName(name);

        profile.Name.Should().Be(name);
    }

    [Fact]
    public void FromName_throws_for_an_unknown_name()
    {
        var act = () => MemberProfile.FromName("Unknown");

        act.Should().Throw<ArgumentOutOfRangeException>();
    }
}
