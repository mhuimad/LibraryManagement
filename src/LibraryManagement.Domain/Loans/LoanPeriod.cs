namespace LibraryManagement.Domain.Loans;

public sealed record LoanPeriod
{
    public DateTimeOffset LoanDate { get; }
    public DateTimeOffset DueDate { get; }

    private LoanPeriod(DateTimeOffset loanDate, DateTimeOffset dueDate)
    {
        LoanDate = loanDate;
        DueDate = dueDate;
    }

    public static LoanPeriod From(DateTimeOffset loanDate, int durationInWeeks) =>
        new(loanDate, loanDate.AddDays(durationInWeeks * 7));
}
