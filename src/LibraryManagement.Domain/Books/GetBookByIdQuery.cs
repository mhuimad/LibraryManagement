using LibraryManagement.Domain.Abstractions;

namespace LibraryManagement.Domain.Books;

public sealed record GetBookByIdQuery(Guid Id) : IQuery<BookDto>;
