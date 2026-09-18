using FluentAssertions;
using LibraryManagement.Domain.Books;
using NSubstitute;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Books;

public class CreateBookCommandHandlerTests
{
    [Fact]
    public async Task Handle_persists_a_new_book_and_returns_its_id()
    {
        var bookRepository = Substitute.For<IBookRepository>();
        var handler = new CreateBookCommandHandler(bookRepository);
        var command = new CreateBookCommand("Clean Code", "Robert C. Martin", 3);

        var bookId = await handler.Handle(command, CancellationToken.None);

        bookId.Should().NotBe(Guid.Empty);
        await bookRepository.Received(1).AddAsync(
            Arg.Is<Book>(b => b.Title == "Clean Code" && b.Author == "Robert C. Martin" && b.TotalCopies == 3 && b.AvailableCopies == 3),
            Arg.Any<CancellationToken>());
    }
}
