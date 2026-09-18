using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Domain.Books;

public sealed class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDto>
{
    private readonly IBookRepository _bookRepository;

    public GetBookByIdQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new BookNotFoundException(request.Id);

        return new BookDto(book.Id, book.Title, book.Author, book.TotalCopies, book.AvailableCopies);
    }
}
