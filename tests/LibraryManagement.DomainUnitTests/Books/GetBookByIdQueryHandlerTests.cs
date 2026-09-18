using FluentAssertions;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Exceptions;
using NSubstitute;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Books;

public class GetBookByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_returns_the_matching_book_as_a_dto()
    {
        var book = Book.Load(Guid.NewGuid(), "Clean Code", "Robert C. Martin", 3, 2);
        var bookRepository = Substitute.For<IBookRepository>();
        bookRepository.GetByIdAsync(book.Id, Arg.Any<CancellationToken>()).Returns(book);
        var handler = new GetBookByIdQueryHandler(bookRepository);

        var result = await handler.Handle(new GetBookByIdQuery(book.Id), CancellationToken.None);

        result.Id.Should().Be(book.Id);
        result.AvailableCopies.Should().Be(2);
    }

    [Fact]
    public async Task Handle_throws_when_the_book_does_not_exist()
    {
        var bookId = Guid.NewGuid();
        var bookRepository = Substitute.For<IBookRepository>();
        bookRepository.GetByIdAsync(bookId, Arg.Any<CancellationToken>()).Returns((Book?)null);
        var handler = new GetBookByIdQueryHandler(bookRepository);

        var act = () => handler.Handle(new GetBookByIdQuery(bookId), CancellationToken.None);

        await act.Should().ThrowAsync<BookNotFoundException>();
    }
}
