namespace LibraryManagement.Domain.Members;

public interface IMemberRepository
{
    Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task AddAsync(Member member, CancellationToken cancellationToken);
}
