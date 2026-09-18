namespace LibraryManagement.Domain.Members;

public sealed record MemberProfile
{
    public string Name { get; }
    public int MaxSimultaneousLoans { get; }
    public int LoanDurationInWeeks { get; }

    private MemberProfile(string name, int maxSimultaneousLoans, int loanDurationInWeeks)
    {
        Name = name;
        MaxSimultaneousLoans = maxSimultaneousLoans;
        LoanDurationInWeeks = loanDurationInWeeks;
    }

    public static MemberProfile Standard => new("Standard", 3, 3);

    public static MemberProfile Student => new("Student", 5, 4);

    public static MemberProfile FromName(string name) => name switch
    {
        "Standard" => Standard,
        "Student" => Student,
        _ => throw new ArgumentOutOfRangeException(nameof(name), name, "Unknown member profile")
    };
}
