using LibraryManagement.Domain.Abstractions;

namespace LibraryManagement.Domain.Loans;

public sealed record GetMemberPenaltiesQuery(Guid MemberId) : IQuery<MemberPenaltiesResult>;
