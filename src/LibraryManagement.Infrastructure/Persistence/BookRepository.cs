using Dapper;
using LibraryManagement.Domain.Books;

namespace LibraryManagement.Infrastructure.Persistence;

public sealed class BookRepository : IBookRepository
{
    private readonly IDbConnectionAccessor _connectionAccessor;

    public BookRepository(IDbConnectionAccessor connectionAccessor)
    {
        _connectionAccessor = connectionAccessor;
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id, Title, Author, TotalCopies, AvailableCopies FROM dbo.Books WHERE Id = @Id";
        var command = new CommandDefinition(sql, new { Id = id }, _connectionAccessor.Transaction, cancellationToken: cancellationToken);
        var row = await _connectionAccessor.Connection.QuerySingleOrDefaultAsync<BookRow>(command);
        return row is null ? null : Book.Load(row.Id, row.Title, row.Author, row.TotalCopies, row.AvailableCopies);
    }

    public async Task<IReadOnlyList<Book>> GetAllAsync(CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id, Title, Author, TotalCopies, AvailableCopies FROM dbo.Books";
        var command = new CommandDefinition(sql, transaction: _connectionAccessor.Transaction, cancellationToken: cancellationToken);
        var rows = await _connectionAccessor.Connection.QueryAsync<BookRow>(command);
        return rows.Select(r => Book.Load(r.Id, r.Title, r.Author, r.TotalCopies, r.AvailableCopies)).ToList();
    }

    public async Task AddAsync(Book book, CancellationToken cancellationToken)
    {
        const string sql = @"INSERT INTO dbo.Books (Id, Title, Author, TotalCopies, AvailableCopies)
                              VALUES (@Id, @Title, @Author, @TotalCopies, @AvailableCopies)";
        var command = new CommandDefinition(
            sql,
            new { book.Id, book.Title, book.Author, book.TotalCopies, book.AvailableCopies },
            _connectionAccessor.Transaction,
            cancellationToken: cancellationToken);
        await _connectionAccessor.Connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(Book book, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE dbo.Books SET AvailableCopies = @AvailableCopies WHERE Id = @Id";
        var command = new CommandDefinition(sql, new { book.Id, book.AvailableCopies }, _connectionAccessor.Transaction, cancellationToken: cancellationToken);
        await _connectionAccessor.Connection.ExecuteAsync(command);
    }

    private sealed record BookRow(Guid Id, string Title, string Author, int TotalCopies, int AvailableCopies);
}
