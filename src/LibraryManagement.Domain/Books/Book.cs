using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Domain.Books;

public sealed class Book
{
    public Guid Id { get; private set; }
    public string Title { get; private set; }
    public string Author { get; private set; }
    public int TotalCopies { get; private set; }
    public int AvailableCopies { get; private set; }

    private Book(Guid id, string title, string author, int totalCopies, int availableCopies)
    {
        Id = id;
        Title = title;
        Author = author;
        TotalCopies = totalCopies;
        AvailableCopies = availableCopies;
    }

    public static Book Create(string title, string author, int totalCopies) =>
        new(Guid.NewGuid(), title, author, totalCopies, totalCopies);

    public static Book Load(Guid id, string title, string author, int totalCopies, int availableCopies) =>
        new(id, title, author, totalCopies, availableCopies);

    public void BorrowCopy()
    {
        if (AvailableCopies <= 0)
            throw new NoCopyAvailableException(Id);

        AvailableCopies--;
    }

    public void ReturnCopy()
    {
        AvailableCopies++;
    }
}
