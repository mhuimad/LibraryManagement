namespace LibraryManagement.Domain.Exceptions;

public sealed class LoanAlreadyReturnedException : Exception
{
    public LoanAlreadyReturnedException(Guid loanId) : base($"Loan {loanId} has already been returned.") { }
}
