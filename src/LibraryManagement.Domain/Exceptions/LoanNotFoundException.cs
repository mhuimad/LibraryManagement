namespace LibraryManagement.Domain.Exceptions;

public sealed class LoanNotFoundException : Exception
{
    public LoanNotFoundException(Guid loanId) : base($"Loan {loanId} was not found.") { }
}
