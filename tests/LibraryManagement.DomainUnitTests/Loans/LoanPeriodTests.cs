using FluentAssertions;
using LibraryManagement.Domain.Loans;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Loans;

public class LoanPeriodTests
{
    [Fact]
    public void From_computes_the_due_date_from_the_duration_in_weeks()
    {
        var loanDate = new DateTimeOffset(2026, 9, 18, 10, 0, 0, TimeSpan.Zero);

        var period = LoanPeriod.From(loanDate, durationInWeeks: 3);

        period.LoanDate.Should().Be(loanDate);
        period.DueDate.Should().Be(new DateTimeOffset(2026, 10, 9, 10, 0, 0, TimeSpan.Zero));
    }
}
