using FluentAssertions;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Members;
using NSubstitute;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Members;

public class GetMemberByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_returns_the_matching_member_as_a_dto()
    {
        var member = Member.Load(Guid.NewGuid(), "Alice", MemberProfile.Standard);
        var memberRepository = Substitute.For<IMemberRepository>();
        memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
        var handler = new GetMemberByIdQueryHandler(memberRepository);

        var result = await handler.Handle(new GetMemberByIdQuery(member.Id), CancellationToken.None);

        result.Id.Should().Be(member.Id);
        result.Name.Should().Be("Alice");
        result.Profile.Should().Be("Standard");
    }

    [Fact]
    public async Task Handle_throws_when_the_member_does_not_exist()
    {
        var memberId = Guid.NewGuid();
        var memberRepository = Substitute.For<IMemberRepository>();
        memberRepository.GetByIdAsync(memberId, Arg.Any<CancellationToken>()).Returns((Member?)null);
        var handler = new GetMemberByIdQueryHandler(memberRepository);

        var act = () => handler.Handle(new GetMemberByIdQuery(memberId), CancellationToken.None);

        await act.Should().ThrowAsync<MemberNotFoundException>();
    }
}
