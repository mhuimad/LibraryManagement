using FluentAssertions;
using LibraryManagement.Domain.Members;
using NSubstitute;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Members;

public class CreateMemberCommandHandlerTests
{
    [Fact]
    public async Task Handle_persists_a_new_member_and_returns_its_id()
    {
        var memberRepository = Substitute.For<IMemberRepository>();
        var handler = new CreateMemberCommandHandler(memberRepository);
        var command = new CreateMemberCommand("Alice", "Standard");

        var memberId = await handler.Handle(command, CancellationToken.None);

        memberId.Should().NotBe(Guid.Empty);
        await memberRepository.Received(1).AddAsync(
            Arg.Is<Member>(m => m.Name == "Alice" && m.Profile == MemberProfile.Standard),
            Arg.Any<CancellationToken>());
    }
}
