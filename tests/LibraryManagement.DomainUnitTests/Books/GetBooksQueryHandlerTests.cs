using FluentAssertions;
using LibraryManagement.Domain.Books;
using NSubstitute;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Books;

public class GetBooksQueryHandlerTests
{
    [Fact]
    public async Task Handle_returns_every_book_as_a_dto()
    {
        var book = Book.Load(Guid.NewGuid(), "Clean Code", "Robert C. Martin", 3, 2);
        var bookRepository = Substitute.For<IBookRepository>();
        bookRepository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(new List<Book> { book });
        var handler = new GetBooksQueryHandler(bookRepository);

        var result = await handler.Handle(new GetBooksQuery(), CancellationToken.None);

        result.Should().ContainSingle(b => b.Id == book.Id && b.Title == "Clean Code" && b.AvailableCopies == 2);
    }
}
