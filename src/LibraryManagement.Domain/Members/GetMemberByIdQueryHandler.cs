using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Domain.Members;

public sealed class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDto>
{
    private readonly IMemberRepository _memberRepository;

    public GetMemberByIdQueryHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<MemberDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _memberRepository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new MemberNotFoundException(request.Id);

        return new MemberDto(member.Id, member.Name, member.Profile.Name);
    }
}
