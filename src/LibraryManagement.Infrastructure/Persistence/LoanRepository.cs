using Dapper;
using LibraryManagement.Domain.Loans;

namespace LibraryManagement.Infrastructure.Persistence;

public sealed class LoanRepository : ILoanRepository
{
    private readonly IDbConnectionAccessor _connectionAccessor;

    public LoanRepository(IDbConnectionAccessor connectionAccessor)
    {
        _connectionAccessor = connectionAccessor;
    }

    public async Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id, BookId, MemberId, LoanDate, DueDate, ReturnDate, PenaltyAmount FROM dbo.Loans WHERE Id = @Id";
        var command = new CommandDefinition(sql, new { Id = id }, _connectionAccessor.Transaction, cancellationToken: cancellationToken);
        var row = await _connectionAccessor.Connection.QuerySingleOrDefaultAsync<LoanRow>(command);
        return row is null
            ? null
            : Loan.Load(row.Id, row.BookId, row.MemberId, row.LoanDate, row.DueDate, row.ReturnDate, row.PenaltyAmount);
    }

    public async Task<int> CountActiveLoansForMemberAsync(Guid memberId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT COUNT(*) FROM dbo.Loans WHERE MemberId = @MemberId AND ReturnDate IS NULL";
        var command = new CommandDefinition(sql, new { MemberId = memberId }, _connectionAccessor.Transaction, cancellationToken: cancellationToken);
        return await _connectionAccessor.Connection.ExecuteScalarAsync<int>(command);
    }

    public async Task<decimal> GetTotalPenaltyForMemberAsync(Guid memberId, CancellationToken cancellationToken)
    {
        const string sql = "SELECT ISNULL(SUM(PenaltyAmount), 0) FROM dbo.Loans WHERE MemberId = @MemberId";
        var command = new CommandDefinition(sql, new { MemberId = memberId }, _connectionAccessor.Transaction, cancellationToken: cancellationToken);
        return await _connectionAccessor.Connection.ExecuteScalarAsync<decimal>(command);
    }

    public async Task AddAsync(Loan loan, CancellationToken cancellationToken)
    {
        const string sql = @"INSERT INTO dbo.Loans (Id, BookId, MemberId, LoanDate, DueDate, ReturnDate, PenaltyAmount)
                              VALUES (@Id, @BookId, @MemberId, @LoanDate, @DueDate, @ReturnDate, @PenaltyAmount)";
        var command = new CommandDefinition(
            sql,
            new { loan.Id, loan.BookId, loan.MemberId, loan.LoanDate, loan.DueDate, loan.ReturnDate, PenaltyAmount = loan.Penalty.Amount },
            _connectionAccessor.Transaction,
            cancellationToken: cancellationToken);
        await _connectionAccessor.Connection.ExecuteAsync(command);
    }

    public async Task UpdateAsync(Loan loan, CancellationToken cancellationToken)
    {
        const string sql = "UPDATE dbo.Loans SET ReturnDate = @ReturnDate, PenaltyAmount = @PenaltyAmount WHERE Id = @Id";
        var command = new CommandDefinition(
            sql,
            new { loan.Id, loan.ReturnDate, PenaltyAmount = loan.Penalty.Amount },
            _connectionAccessor.Transaction,
            cancellationToken: cancellationToken);
        await _connectionAccessor.Connection.ExecuteAsync(command);
    }

    private sealed record LoanRow(Guid Id, Guid BookId, Guid MemberId, DateTimeOffset LoanDate, DateTimeOffset DueDate, DateTimeOffset? ReturnDate, decimal PenaltyAmount);
}
