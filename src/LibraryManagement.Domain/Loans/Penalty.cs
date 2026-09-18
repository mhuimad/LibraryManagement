namespace LibraryManagement.Domain.Loans;

public sealed record Penalty
{
    private const decimal DailyRate = 0.20m;
    private const decimal MaxAmount = 10m;

    public decimal Amount { get; }

    private Penalty(decimal amount) => Amount = amount;

    public static Penalty None => new(0m);

    public static Penalty ForLateDays(int lateDays)
    {
        if (lateDays <= 0)
            return None;

        return new Penalty(Math.Min(lateDays * DailyRate, MaxAmount));
    }

    public static Penalty FromAmount(decimal amount) => new(amount);
}
