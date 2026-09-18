using FluentAssertions;
using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Exceptions;
using Xunit;

namespace LibraryManagement.DomainUnitTests.Books;

public class BookTests
{
    [Fact]
    public void Create_sets_available_copies_equal_to_total_copies()
    {
        var book = Book.Create("Clean Code", "Robert C. Martin", 3);

        book.Id.Should().NotBe(Guid.Empty);
        book.Title.Should().Be("Clean Code");
        book.Author.Should().Be("Robert C. Martin");
        book.TotalCopies.Should().Be(3);
        book.AvailableCopies.Should().Be(3);
    }

    [Fact]
    public void BorrowCopy_decrements_available_copies()
    {
        var book = Book.Create("Clean Code", "Robert C. Martin", 2);

        book.BorrowCopy();

        book.AvailableCopies.Should().Be(1);
    }

    [Fact]
    public void BorrowCopy_throws_when_no_copy_is_available()
    {
        var book = Book.Load(Guid.NewGuid(), "Clean Code", "Robert C. Martin", 1, 0);

        var act = book.BorrowCopy;

        act.Should().Throw<NoCopyAvailableException>();
    }

    [Fact]
    public void ReturnCopy_increments_available_copies()
    {
        var book = Book.Load(Guid.NewGuid(), "Clean Code", "Robert C. Martin", 2, 1);

        book.ReturnCopy();

        book.AvailableCopies.Should().Be(2);
    }
}
