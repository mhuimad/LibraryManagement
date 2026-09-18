namespace LibraryManagement.Domain.Exceptions;

public sealed class BookNotFoundException : Exception
{
    public BookNotFoundException(Guid bookId) : base($"Book {bookId} was not found.") { }
}
