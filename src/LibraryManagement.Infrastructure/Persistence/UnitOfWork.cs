using System.Data;
using LibraryManagement.Domain.Abstractions;
using Microsoft.Data.SqlClient;

namespace LibraryManagement.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork, IDbConnectionAccessor, IDisposable
{
    private readonly SqlConnection _connection;
    private SqlTransaction? _transaction;

    public UnitOfWork(string connectionString)
    {
        _connection = new SqlConnection(connectionString);
        _connection.Open();
    }

    public IDbConnection Connection => _connection;
    public IDbTransaction? Transaction => _transaction;

    public void BeginTransaction() => _transaction = _connection.BeginTransaction();

    public void Commit()
    {
        _transaction?.Commit();
        _transaction = null;
    }

    public void Rollback()
    {
        _transaction?.Rollback();
        _transaction = null;
    }

    public void Dispose() => _connection.Dispose();
}
