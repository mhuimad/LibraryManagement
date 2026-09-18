using FluentAssertions;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Loans;
using Microsoft.Extensions.Time.Testing;
using NSubstitute;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Loans;

public class ReturnBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_returns_the_loan_and_increments_available_copies()
    {
        var loanDate = new DateTimeOffset(2026, 9, 1, 10, 0, 0, TimeSpan.Zero);
        var timeProvider = new FakeTimeProvider(loanDate.AddDays(26));
        var book = Book.Load(Guid.NewGuid(), "Clean Code", "Robert C. Martin", 2, 1);
        var loan = Loan.Create(book.Id, Guid.NewGuid(), LoanPeriod.From(loanDate, 3));

        var loanRepository = Substitute.For<ILoanRepository>();
        loanRepository.GetByIdAsync(loan.Id, Arg.Any<CancellationToken>()).Returns(loan);
        var bookRepository = Substitute.For<IBookRepository>();
        bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);

        var handler = new ReturnBookCommandHandler(loanRepository, bookRepository, timeProvider);

        var result = await handler.Handle(new ReturnBookCommand(loan.Id), CancellationToken.None);

        result.LateDays.Should().Be(5);
        result.PenaltyAmount.Should().Be(1.00m);
        book.AvailableCopies.Should().Be(2);
        await loanRepository.Received(1).UpdateAsync(loan, Arg.Any<CancellationToken>());
        await bookRepository.Received(1).UpdateAsync(book, Arg.Any<CancellationToken>());
    }
}
