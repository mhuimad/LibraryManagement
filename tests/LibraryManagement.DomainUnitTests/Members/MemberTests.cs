using FluentAssertions;
using LibraryManagement.Domain.Members;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Members;

public class MemberTests
{
    [Fact]
    public void Create_assigns_a_new_id_name_and_profile()
    {
        var member = Member.Create("Alice", MemberProfile.Standard);

        member.Id.Should().NotBe(Guid.Empty);
        member.Name.Should().Be("Alice");
        member.Profile.Should().Be(MemberProfile.Standard);
    }
}
