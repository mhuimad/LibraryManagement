using LibraryManagement.Domain.Abstractions;

namespace LibraryManagement.Domain.Loans;

public sealed record ReturnBookCommand(Guid LoanId) : ICommand<ReturnBookResult>;
