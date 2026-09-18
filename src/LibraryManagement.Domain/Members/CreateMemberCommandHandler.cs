using MediatR;

namespace LibraryManagement.Domain.Members;

public sealed class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Guid>
{
    private readonly IMemberRepository _memberRepository;

    public CreateMemberCommandHandler(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<Guid> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = Member.Create(request.Name, MemberProfile.FromName(request.Profile));
        await _memberRepository.AddAsync(member, cancellationToken);
        return member.Id;
    }
}
