namespace LibraryManagement.Domain.Abstractions;

public interface IUnitOfWork
{
    void BeginTransaction();
    void Commit();
    void Rollback();
}
