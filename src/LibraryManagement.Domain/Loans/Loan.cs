using LibraryManagement.Domain.Exceptions;

namespace LibraryManagement.Domain.Loans;

public sealed class Loan
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public Guid MemberId { get; private set; }
    public DateTimeOffset LoanDate { get; private set; }
    public DateTimeOffset DueDate { get; private set; }
    public DateTimeOffset? ReturnDate { get; private set; }
    public Penalty Penalty { get; private set; }

    private Loan(Guid id, Guid bookId, Guid memberId, DateTimeOffset loanDate, DateTimeOffset dueDate, DateTimeOffset? returnDate, Penalty penalty)
    {
        Id = id;
        BookId = bookId;
        MemberId = memberId;
        LoanDate = loanDate;
        DueDate = dueDate;
        ReturnDate = returnDate;
        Penalty = penalty;
    }

    public static Loan Create(Guid bookId, Guid memberId, LoanPeriod period) =>
        new(Guid.NewGuid(), bookId, memberId, period.LoanDate, period.DueDate, null, Penalty.None);

    public static Loan Load(Guid id, Guid bookId, Guid memberId, DateTimeOffset loanDate, DateTimeOffset dueDate, DateTimeOffset? returnDate, decimal penaltyAmount) =>
        new(id, bookId, memberId, loanDate, dueDate, returnDate, Penalty.FromAmount(penaltyAmount));

    public void Return(DateTimeOffset returnDate)
    {
        if (ReturnDate is not null)
            throw new LoanAlreadyReturnedException(Id);

        ReturnDate = returnDate;
        var lateDays = (int)Math.Max(0, (returnDate.Date - DueDate.Date).Days);
        Penalty = Penalty.ForLateDays(lateDays);
    }
}
