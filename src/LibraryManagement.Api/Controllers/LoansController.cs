using LibraryManagement.Api.Contracts;
using LibraryManagement.Domain.Loans;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/loans")]
public sealed class LoansController : ControllerBase
{
    private readonly IMediator _mediator;

    public LoansController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<BorrowBookResult>> Borrow(BorrowBookRequest request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new BorrowBookCommand(request.MemberId, request.BookId), cancellationToken);
        return Ok(result);
    }

    [HttpPost("{id:guid}/return")]
    public async Task<ActionResult<ReturnBookResult>> Return(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReturnBookCommand(id), cancellationToken);
        return Ok(result);
    }
}
