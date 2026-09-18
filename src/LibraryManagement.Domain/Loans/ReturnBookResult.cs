namespace LibraryManagement.Domain.Loans;

public sealed record ReturnBookResult(Guid LoanId, int LateDays, decimal PenaltyAmount);
