using LibraryManagement.Domain.Books;
using LibraryManagement.Domain.Exceptions;
using MediatR;

namespace LibraryManagement.Domain.Loans;

public sealed class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, ReturnBookResult>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IBookRepository _bookRepository;
    private readonly TimeProvider _timeProvider;

    public ReturnBookCommandHandler(ILoanRepository loanRepository, IBookRepository bookRepository, TimeProvider timeProvider)
    {
        _loanRepository = loanRepository;
        _bookRepository = bookRepository;
        _timeProvider = timeProvider;
    }

    public async Task<ReturnBookResult> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.LoanId, cancellationToken)
            ?? throw new LoanNotFoundException(request.LoanId);

        var now = _timeProvider.GetUtcNow();
        loan.Return(now);

        var book = await _bookRepository.GetByIdAsync(loan.BookId, cancellationToken)
            ?? throw new BookNotFoundException(loan.BookId);
        book.ReturnCopy();

        await _loanRepository.UpdateAsync(loan, cancellationToken);
        await _bookRepository.UpdateAsync(book, cancellationToken);

        var lateDays = (int)Math.Max(0, (now.Date - loan.DueDate.Date).Days);
        return new ReturnBookResult(loan.Id, lateDays, loan.Penalty.Amount);
    }
}
