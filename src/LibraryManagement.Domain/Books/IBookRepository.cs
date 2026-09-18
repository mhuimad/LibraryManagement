namespace LibraryManagement.Domain.Books;

public interface IBookRepository
{
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(Book book, CancellationToken cancellationToken);
    Task UpdateAsync(Book book, CancellationToken cancellationToken);
}
