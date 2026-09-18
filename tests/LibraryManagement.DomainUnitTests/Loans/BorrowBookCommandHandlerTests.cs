using FluentAssertions;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Loans;

public class BorrowBookCommandHandlerTests
{
    private static readonly DateTimeOffset Now = new(2026, 9, 18, 10, 0, 0, TimeSpan.Zero);

    private static (BorrowBookCommandHandler Handler, IBookRepository BookRepository, ILoanRepository LoanRepository, Book Book, Member Member) CreateSut(int availableCopies, int activeLoans)
    {
        var timeProvider = new FakeTimeProvider(Now);
        var book = Book.Load(Guid.NewGuid(), "Clean Code", "Robert C. Martin", 2, availableCopies);
        var member = Member.Load(Guid.NewGuid(), "Alice", MemberProfile.Standard);

        var bookRepository = Substitute.For<IBookRepository>();
        bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);
        var memberRepository = Substitute.For<IMemberRepository>();
        memberRepository.GetByIdAsync(member.Id, Arg.Any<CancellationToken>()).Returns(member);
        var loanRepository = Substitute.For<ILoanRepository>();
        loanRepository.CountActiveLoansForMemberAsync(member.Id, Arg.Any<CancellationToken>()).Returns(activeLoans);

        var handler = new BorrowBookCommandHandler(bookRepository, memberRepository, loanRepository, timeProvider);
        return (handler, bookRepository, loanRepository, book, member);
    }

    [Fact]
    public async Task Handle_creates_a_loan_and_decrements_available_copies()
    {
        var sut = CreateSut(availableCopies: 2, activeLoans: 0);

        var result = await sut.Handler.Handle(new BorrowBookCommand(sut.Member.Id, sut.Book.Id), CancellationToken.None);

        result.DueDate.Should().Be(Now.AddDays(21));
        sut.Book.AvailableCopies.Should().Be(1);
        await sut.BookRepository.Received(1).UpdateAsync(sut.Book, Arg.Any<CancellationToken>());
        await sut.LoanRepository.Received(1).AddAsync(
            Arg.Is<Loan>(l => l.BookId == sut.Book.Id && l.MemberId == sut.Member.Id),
            Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_throws_when_the_member_has_reached_its_loan_quota()
    {
        var sut = CreateSut(availableCopies: 2, activeLoans: 3);

        var act = () => sut.Handler.Handle(new BorrowBookCommand(sut.Member.Id, sut.Book.Id), CancellationToken.None);

        await act.Should().ThrowAsync<LoanQuotaExceededException>();
    }

    [Fact]
    public async Task Handle_throws_when_no_copy_is_available()
    {
        var sut = CreateSut(availableCopies: 0, activeLoans: 0);

        var act = () => sut.Handler.Handle(new BorrowBookCommand(sut.Member.Id, sut.Book.Id), CancellationToken.None);

        await act.Should().ThrowAsync<NoCopyAvailableException>();
    }
}
