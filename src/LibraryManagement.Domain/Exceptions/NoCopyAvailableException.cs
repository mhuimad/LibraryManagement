namespace LibraryManagement.Domain.Exceptions;

public sealed class NoCopyAvailableException : Exception
{
    public NoCopyAvailableException(Guid bookId) : base($"Book {bookId} has no available copy.") { }
}
