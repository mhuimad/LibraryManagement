namespace LibraryManagement.Domain.Exceptions;

public sealed class MemberNotFoundException : Exception
{
    public MemberNotFoundException(Guid memberId) : base($"Member {memberId} was not found.") { }
}
