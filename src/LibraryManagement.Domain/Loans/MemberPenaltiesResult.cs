namespace LibraryManagement.Domain.Loans;

public sealed record MemberPenaltiesResult(Guid MemberId, decimal TotalPenaltyAmount);
