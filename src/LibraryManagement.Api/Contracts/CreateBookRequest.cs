namespace LibraryManagement.Api.Contracts;

public sealed record CreateBookRequest(string Title, string Author, int TotalCopies);
