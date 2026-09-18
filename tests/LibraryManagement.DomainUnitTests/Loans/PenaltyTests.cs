using FluentAssertions;
using LibraryManagement.Domain.Loans;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Loans;

public class PenaltyTests
{
    [Fact]
    public void None_has_a_zero_amount()
    {
        Penalty.None.Amount.Should().Be(0m);
    }

    [Theory]
    [InlineData(0, 0.0)]
    [InlineData(-1, 0.0)]
    [InlineData(1, 0.20)]
    [InlineData(5, 1.00)]
    [InlineData(50, 10.00)]
    [InlineData(100, 10.00)]
    public void ForLateDays_computes_the_capped_penalty(int lateDays, decimal expectedAmount)
    {
        var penalty = Penalty.ForLateDays(lateDays);

        penalty.Amount.Should().Be(expectedAmount);
    }

    [Fact]
    public void FromAmount_wraps_a_stored_amount_as_is()
    {
        var penalty = Penalty.FromAmount(3.40m);

        penalty.Amount.Should().Be(3.40m);
    }
}
