using MediatR;

namespace LibraryManagement.Domain.Books;

public sealed class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, IReadOnlyList<BookDto>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IReadOnlyList<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _bookRepository.GetAllAsync(cancellationToken);
        return books
            .Select(b => new BookDto(b.Id, b.Title, b.Author, b.TotalCopies, b.AvailableCopies))
            .ToList();
    }
}
