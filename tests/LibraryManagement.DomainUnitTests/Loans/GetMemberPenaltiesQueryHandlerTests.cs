using FluentAssertions;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;
using NSubstitute;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Loans;

public class GetMemberPenaltiesQueryHandlerTests
{
    [Fact]
    public async Task Handle_returns_the_total_penalty_amount_for_the_member()
    {
        var member = Member.Load(Guid.NewGuid(), "Alice", MemberProfile.Standard);
        var memberRepository = Substitute.For<IMemberRepository>();
        memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
        var loanRepository = Substitute.For<ILoanRepository>();
        loanRepository.GetTotalPenaltyForMemberAsync(member.Id, Arg.Any<CancellationToken>()).Returns(4.60m);

        var handler = new GetMemberPenaltiesQueryHandler(memberRepository, loanRepository);

        var result = await handler.Handle(new GetMemberPenaltiesQuery(member.Id), CancellationToken.None);

        result.TotalPenaltyAmount.Should().Be(4.60m);
    }

    [Fact]
    public async Task Handle_throws_when_the_member_does_not_exist()
    {
        var memberId = Guid.NewGuid();
        var memberRepository = Substitute.For<IMemberRepository>();
        memberRepository.GetByIdAsync(memberId, Arg.Any<CancellationToken>()).Returns((Member?)null);
        var loanRepository = Substitute.For<ILoanRepository>();

        var handler = new GetMemberPenaltiesQueryHandler(memberRepository, loanRepository);

        var act = () => handler.Handle(new GetMemberPenaltiesQuery(memberId), CancellationToken.None);

        await act.Should().ThrowAsync<MemberNotFoundException>();
    }
}
