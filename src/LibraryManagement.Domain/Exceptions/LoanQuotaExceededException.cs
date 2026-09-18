namespace LibraryManagement.Domain.Exceptions;

public sealed class LoanQuotaExceededException : Exception
{
    public LoanQuotaExceededException(Guid memberId, int maxSimultaneousLoans)
        : base($"Member {memberId} has reached its quota of {maxSimultaneousLoans} simultaneous loans.") { }
}
