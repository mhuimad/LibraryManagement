namespace LibraryManagement.Domain.Books;

public sealed record BookDto(Guid Id, string Title, string Author, int TotalCopies, int AvailableCopies);
