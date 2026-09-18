namespace LibraryManagement.Api.Contracts;

public sealed record BorrowBookRequest(Guid MemberId, Guid BookId);
