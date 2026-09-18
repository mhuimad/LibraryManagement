using LibraryManagement.Domain.Abstractions;

namespace LibraryManagement.Domain.Books;

public sealed record CreateBookCommand(string Title, string Author, int TotalCopies) : ICommand<Guid>;
