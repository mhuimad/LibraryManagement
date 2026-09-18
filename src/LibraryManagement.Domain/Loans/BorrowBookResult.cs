namespace LibraryManagement.Domain.Loans;

public sealed record BorrowBookResult(Guid LoanId, DateTimeOffset DueDate);
