using LibraryManagement.Domain.Abstractions;
using MediatR;

namespace LibraryManagement.Infrastructure.Behaviors;

public sealed class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _unitOfWork.BeginTransaction();
        try
        {
            var response = await next();
            _unitOfWork.Commit();
            return response;
        }
        catch
        {
            _unitOfWork.Rollback();
            throw;
        }
    }
}
