using MediatR;

namespace LibraryManagement.Domain.Abstractions;

public interface ICommand<TResponse> : IRequest<TResponse> { }
