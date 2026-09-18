using LibraryManagement.Domain.Abstractions;

namespace LibraryManagement.Domain.Members;

public sealed record GetMemberByIdQuery(Guid Id) : IQuery<MemberDto>;
