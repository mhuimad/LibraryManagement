using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Members;
using MediatR;

namespace LibraryManagement.Domain.Loans;

public sealed class GetMemberPenaltiesQueryHandler : IRequestHandler<GetMemberPenaltiesQuery, MemberPenaltiesResult>
{
    private readonly IMemberRepository _memberRepository;
    private readonly ILoanRepository _loanRepository;

    public GetMemberPenaltiesQueryHandler(IMemberRepository memberRepository, ILoanRepository loanRepository)
    {
        _memberRepository = memberRepository;
        _loanRepository = loanRepository;
    }

    public async Task<MemberPenaltiesResult> Handle(GetMemberPenaltiesQuery request, CancellationToken cancellationToken)
    {
        _ = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new MemberNotFoundException(request.MemberId);

        var total = await _loanRepository.GetTotalPenaltyForMemberAsync(request.MemberId, cancellationToken);
        return new MemberPenaltiesResult(request.MemberId, total);
    }
}
