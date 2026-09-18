using MediatR;

namespace LibraryManagement.Domain.Abstractions;

public interface IQuery<TResponse> : IRequest<TResponse> { }
