using System.Data;

namespace LibraryManagement.Infrastructure.Persistence;

public interface IDbConnectionAccessor
{
    IDbConnection Connection { get; }
    IDbTransaction? Transaction { get; }
}
