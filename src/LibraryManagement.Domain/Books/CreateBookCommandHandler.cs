using MediatR;

namespace LibraryManagement.Domain.Books;

public sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, Guid>
{
    private readonly IBookRepository _bookRepository;

    public CreateBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = Book.Create(request.Title, request.Author, request.TotalCopies);
        await _bookRepository.AddAsync(book, cancellationToken);
        return book.Id;
    }
}
