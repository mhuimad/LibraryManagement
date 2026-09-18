using LibraryManagement.Domain.Abstractions;

namespace LibraryManagement.Domain.Books;

public sealed record GetBooksQuery : IQuery<IReadOnlyList<BookDto>>;
