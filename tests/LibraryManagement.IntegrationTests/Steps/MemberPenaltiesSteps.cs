using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using LibraryManagement.IntegrationTests.Hooks;
using Microsoft.Data.SqlClient;
using Reqnroll;

namespace LibraryManagement.IntegrationTests.Steps;

[Binding]
public sealed class MemberPenaltiesSteps
{
    private readonly HttpClient _client = new ApiFactory().CreateClient();
    private Guid _memberId;
    private MemberPenaltiesResultDto _result = new(Guid.Empty, 0m);

    [Given(@"a member with a return (\d+) days late")]
    public async Task GivenAMemberWithALateReturn(int daysLate)
    {
        var memberResponse = await _client.PostAsJsonAsync("/api/members", new { Name = "Alice", Profile = "Standard" });
        _memberId = await memberResponse.Content.ReadFromJsonAsync<Guid>();

        await BorrowAndReturnLateAsync(daysLate);
    }

    [Given(@"the same member with another return (\d+) days late")]
    public async Task GivenTheSameMemberWithAnotherLateReturn(int daysLate)
    {
        await BorrowAndReturnLateAsync(daysLate);
    }

    [Given(@"a standard member with no loans")]
    public async Task GivenAStandardMemberWithNoLoans()
    {
        var memberResponse = await _client.PostAsJsonAsync("/api/members", new { Name = "Bob", Profile = "Standard" });
        _memberId = await memberResponse.Content.ReadFromJsonAsync<Guid>();
    }

    [When(@"the total penalties for the member are requested")]
    public async Task WhenTheTotalPenaltiesAreRequested()
    {
        _result = (await _client.GetFromJsonAsync<MemberPenaltiesResultDto>($"/api/members/{_memberId}/penalties"))!;
    }

    [Then(@"the total penalty amount is (.*)")]
    public void ThenTheTotalPenaltyAmountIs(decimal expectedAmount)
    {
        _result.TotalPenaltyAmount.Should().Be(expectedAmount);
    }

    private async Task BorrowAndReturnLateAsync(int daysLate)
    {
        var bookResponse = await _client.PostAsJsonAsync("/api/books", new { Title = "Clean Code", Author = "Robert C. Martin", TotalCopies = 1 });
        var bookId = await bookResponse.Content.ReadFromJsonAsync<Guid>();

        var loanResponse = await _client.PostAsJsonAsync("/api/loans", new { MemberId = _memberId, BookId = bookId });
        var loanResult = await loanResponse.Content.ReadFromJsonAsync<BorrowBookResultDto>();

        await using var connection = new SqlConnection(DatabaseHooks.ConnectionString);
        await connection.ExecuteAsync(
            "UPDATE dbo.Loans SET DueDate = @DueDate WHERE Id = @Id",
            new { DueDate = DateTimeOffset.UtcNow.AddDays(-daysLate), Id = loanResult!.LoanId });

        await _client.PostAsync($"/api/loans/{loanResult.LoanId}/return", null);
    }

    private sealed record BorrowBookResultDto(Guid LoanId, DateTimeOffset DueDate);
    private sealed record MemberPenaltiesResultDto(Guid MemberId, decimal TotalPenaltyAmount);
}
