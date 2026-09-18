using Dapper;
using LibraryManagement.Domain.Members;

namespace LibraryManagement.Infrastructure.Persistence;

public sealed class MemberRepository : IMemberRepository
{
    private readonly IDbConnectionAccessor _connectionAccessor;

    public MemberRepository(IDbConnectionAccessor connectionAccessor)
    {
        _connectionAccessor = connectionAccessor;
    }

    public async Task<Member?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        const string sql = "SELECT Id, Name, Profile FROM dbo.Members WHERE Id = @Id";
        var command = new CommandDefinition(sql, new { Id = id }, _connectionAccessor.Transaction, cancellationToken: cancellationToken);
        var row = await _connectionAccessor.Connection.QuerySingleOrDefaultAsync<MemberRow>(command);
        return row is null ? null : Member.Load(row.Id, row.Name, MemberProfile.FromName(row.Profile));
    }

    public async Task AddAsync(Member member, CancellationToken cancellationToken)
    {
        const string sql = "INSERT INTO dbo.Members (Id, Name, Profile) VALUES (@Id, @Name, @Profile)";
        var command = new CommandDefinition(
            sql,
            new { member.Id, member.Name, Profile = member.Profile.Name },
            _connectionAccessor.Transaction,
            cancellationToken: cancellationToken);
        await _connectionAccessor.Connection.ExecuteAsync(command);
    }

    private sealed record MemberRow(Guid Id, string Name, string Profile);
}
