namespace LibraryManagement.Domain.Loans;

public interface ILoanRepository
{
    Task<Loan?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<int> CountActiveLoansForMemberAsync(Guid memberId, CancellationToken cancellationToken);
    Task<decimal> GetTotalPenaltyForMemberAsync(Guid memberId, CancellationToken cancellationToken);
    Task AddAsync(Loan loan, CancellationToken cancellationToken);
    Task UpdateAsync(Loan loan, CancellationToken cancellationToken);
}
