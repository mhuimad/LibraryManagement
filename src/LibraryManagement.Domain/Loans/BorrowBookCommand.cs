using LibraryManagement.Domain.Abstractions;

namespace LibraryManagement.Domain.Loans;

public sealed record BorrowBookCommand(Guid MemberId, Guid BookId) : ICommand<BorrowBookResult>;
