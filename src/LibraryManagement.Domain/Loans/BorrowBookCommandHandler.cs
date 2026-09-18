using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Exceptions;
using LibraryManagement.Domain.Members;
using MediatR;

namespace LibraryManagement.Domain.Loans;

public sealed class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, BorrowBookResult>
{
    private readonly IBookRepository _bookRepository;
    private readonly IMemberRepository _memberRepository;
    private readonly ILoanRepository _loanRepository;
    private readonly TimeProvider _timeProvider;

    public BorrowBookCommandHandler(
        IBookRepository bookRepository,
        IMemberRepository memberRepository,
        ILoanRepository loanRepository,
        TimeProvider timeProvider)
    {
        _bookRepository = bookRepository;
        _memberRepository = memberRepository;
        _loanRepository = loanRepository;
        _timeProvider = timeProvider;
    }

    public async Task<BorrowBookResult> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetByIdAsync(request.BookId, cancellationToken)
            ?? throw new BookNotFoundException(request.BookId);
        var member = await _memberRepository.GetByIdAsync(request.MemberId, cancellationToken)
            ?? throw new MemberNotFoundException(request.MemberId);

        var activeLoansCount = await _loanRepository.CountActiveLoansForMemberAsync(request.MemberId, cancellationToken);
        if (activeLoansCount >= member.Profile.MaxSimultaneousLoans)
            throw new LoanQuotaExceededException(member.Id, member.Profile.MaxSimultaneousLoans);

        book.BorrowCopy();

        var now = _timeProvider.GetUtcNow();
        var loan = Loan.Create(book.Id, member.Id, LoanPeriod.From(now, member.Profile.LoanDurationInWeeks));

        await _bookRepository.UpdateAsync(book, cancellationToken);
        await _loanRepository.AddAsync(loan, cancellationToken);

        return new BorrowBookResult(loan.Id, loan.DueDate);
    }
}
