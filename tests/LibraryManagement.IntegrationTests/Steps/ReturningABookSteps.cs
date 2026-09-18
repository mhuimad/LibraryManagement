using System.Net;
using System.Net.Http.Json;
using Dapper;
using FluentAssertions;
using LibraryManagement.IntegrationTests.Hooks;
using Microsoft.Data.SqlClient;
using Reqnroll;

namespace LibraryManagement.IntegrationTests.Steps;

[Binding]
public sealed class ReturningABookSteps
{
    private readonly HttpClient _client = new ApiFactory().CreateClient();
    private Guid _loanId;
    private HttpResponseMessage _response = new(HttpStatusCode.OK);

    [Given(@"a borrowed book due today")]
    public async Task GivenABorrowedBookDueToday()
    {
        await BorrowAndSetDueDateAsync(DateTimeOffset.UtcNow);
    }

    [Given(@"a borrowed book due (\d+) days ago")]
    public async Task GivenABorrowedBookDueDaysAgo(int daysAgo)
    {
        await BorrowAndSetDueDateAsync(DateTimeOffset.UtcNow.AddDays(-daysAgo));
    }

    [Given(@"the book has already been returned")]
    public async Task GivenTheBookHasAlreadyBeenReturned()
    {
        await _client.PostAsync($"/api/loans/{_loanId}/return", null);
    }

    [When(@"the member returns the book")]
    public async Task WhenTheMemberReturnsTheBook()
    {
        _response = await _client.PostAsync($"/api/loans/{_loanId}/return", null);
    }

    [Then(@"the return is accepted with no penalty")]
    public async Task ThenTheReturnIsAcceptedWithNoPenalty()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await _response.Content.ReadFromJsonAsync<ReturnBookResultDto>();
        result!.PenaltyAmount.Should().Be(0m);
    }

    [Then(@"the return is accepted with a penalty of (.*)")]
    public async Task ThenTheReturnIsAcceptedWithAPenaltyOf(decimal expectedPenalty)
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);
        var result = await _response.Content.ReadFromJsonAsync<ReturnBookResultDto>();
        result!.PenaltyAmount.Should().Be(expectedPenalty);
    }

    [Then(@"the return is rejected with status (\d+)")]
    public void ThenTheReturnIsRejected(int statusCode)
    {
        ((int)_response.StatusCode).Should().Be(statusCode);
    }

    private async Task BorrowAndSetDueDateAsync(DateTimeOffset dueDate)
    {
        var memberResponse = await _client.PostAsJsonAsync("/api/members", new { Name = "Alice", Profile = "Standard" });
        var memberId = await memberResponse.Content.ReadFromJsonAsync<Guid>();

        var bookResponse = await _client.PostAsJsonAsync("/api/books", new { Title = "Clean Code", Author = "Robert C. Martin", TotalCopies = 1 });
        var bookId = await bookResponse.Content.ReadFromJsonAsync<Guid>();

        var loanResponse = await _client.PostAsJsonAsync("/api/loans", new { MemberId = memberId, BookId = bookId });
        var loanResult = await loanResponse.Content.ReadFromJsonAsync<BorrowBookResultDto>();
        _loanId = loanResult!.LoanId;

        await using var connection = new SqlConnection(DatabaseHooks.ConnectionString);
        await connection.ExecuteAsync("UPDATE dbo.Loans SET DueDate = @DueDate WHERE Id = @Id", new { DueDate = dueDate, Id = _loanId });
    }

    private sealed record BorrowBookResultDto(Guid LoanId, DateTimeOffset DueDate);
    private sealed record ReturnBookResultDto(Guid LoanId, int LateDays, decimal PenaltyAmount);
}
