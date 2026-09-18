using FluentAssertions;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Loans;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Loans;

public class LoanTests
{
    private static readonly Guid BookId = Guid.NewGuid();
    private static readonly Guid MemberId = Guid.NewGuid();
    private static readonly LoanPeriod Period = LoanPeriod.From(new DateTimeOffset(2026, 9, 1, 0, 0, 0, TimeSpan.Zero), 3);

    [Fact]
    public void Create_sets_loan_and_due_dates_from_the_period_and_no_penalty()
    {
        var loan = Loan.Create(BookId, MemberId, Period);

        loan.Id.Should().NotBe(Guid.Empty);
        loan.BookId.Should().Be(BookId);
        loan.MemberId.Should().Be(MemberId);
        loan.LoanDate.Should().Be(Period.LoanDate);
        loan.DueDate.Should().Be(Period.DueDate);
        loan.ReturnDate.Should().BeNull();
        loan.Penalty.Should().Be(Penalty.None);
    }

    [Fact]
    public void Return_on_time_sets_the_return_date_with_no_penalty()
    {
        var loan = Loan.Create(BookId, MemberId, Period);

        loan.Return(Period.DueDate);

        loan.ReturnDate.Should().Be(Period.DueDate);
        loan.Penalty.Should().Be(Penalty.None);
    }

    [Fact]
    public void Return_five_days_late_applies_the_matching_penalty()
    {
        var loan = Loan.Create(BookId, MemberId, Period);

        loan.Return(Period.DueDate.AddDays(5));

        loan.Penalty.Amount.Should().Be(1.00m);
    }

    [Fact]
    public void Return_throws_when_the_loan_was_already_returned()
    {
        var loan = Loan.Create(BookId, MemberId, Period);
        loan.Return(Period.DueDate);

        var act = () => loan.Return(Period.DueDate.AddDays(1));

        act.Should().Throw<LoanAlreadyReturnedException>();
    }
}
