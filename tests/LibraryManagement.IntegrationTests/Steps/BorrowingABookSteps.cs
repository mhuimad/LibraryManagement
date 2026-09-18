using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using LibraryManagement.IntegrationTests.Hooks;
using Reqnroll;

namespace LibraryManagement.IntegrationTests.Steps;

[Binding]
public sealed class BorrowingABookSteps
{
    private readonly HttpClient _client = new ApiFactory().CreateClient();
    private Guid _bookId;
    private Guid _memberId;
    private HttpResponseMessage _response = new(HttpStatusCode.OK);

    [Given(@"a book titled ""(.*)"" by ""(.*)"" with (\d+) available copies")]
    public async Task GivenABook(string title, string author, int availableCopies)
    {
        var totalCopies = Math.Max(availableCopies, 1);
        var createResponse = await _client.PostAsJsonAsync("/api/books", new { Title = title, Author = author, TotalCopies = totalCopies });
        _bookId = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var copiesToBorrow = totalCopies - availableCopies;
        for (var i = 0; i < copiesToBorrow; i++)
        {
            var member = await CreateMemberAsync("Standard");
            await _client.PostAsJsonAsync("/api/loans", new { MemberId = member, BookId = _bookId });
        }
    }

    [Given(@"a standard member named ""(.*)""")]
    public async Task GivenAStandardMember(string name)
    {
        _memberId = await CreateMemberAsync("Standard", name);
    }

    [Given(@"a standard member named ""(.*)"" with (\d+) active loans")]
    public async Task GivenAStandardMemberWithActiveLoans(string name, int activeLoans)
    {
        _memberId = await CreateMemberAsync("Standard", name);

        for (var i = 0; i < activeLoans; i++)
        {
            var createResponse = await _client.PostAsJsonAsync("/api/books", new { Title = $"Filler {i}", Author = "Filler", TotalCopies = 1 });
            var fillerBookId = await createResponse.Content.ReadFromJsonAsync<Guid>();
            await _client.PostAsJsonAsync("/api/loans", new { MemberId = _memberId, BookId = fillerBookId });
        }
    }

    [When(@"the member borrows the book")]
    public async Task WhenTheMemberBorrowsTheBook()
    {
        _response = await _client.PostAsJsonAsync("/api/loans", new { MemberId = _memberId, BookId = _bookId });
    }

    [Then(@"the loan is accepted")]
    public void ThenTheLoanIsAccepted()
    {
        _response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Then(@"the loan is rejected with status (\d+)")]
    public void ThenTheLoanIsRejected(int statusCode)
    {
        ((int)_response.StatusCode).Should().Be(statusCode);
    }

    [Then(@"the book has (\d+) available copy")]
    public async Task ThenTheBookHasAvailableCopies(int expectedAvailableCopies)
    {
        var book = await _client.GetFromJsonAsync<BookDto>($"/api/books/{_bookId}");
        book!.AvailableCopies.Should().Be(expectedAvailableCopies);
    }

    private async Task<Guid> CreateMemberAsync(string profile, string name = "Member")
    {
        var response = await _client.PostAsJsonAsync("/api/members", new { Name = name, Profile = profile });
        return await response.Content.ReadFromJsonAsync<Guid>();
    }

    private sealed record BookDto(Guid Id, string Title, string Author, int TotalCopies, int AvailableCopies);
}
