using LibraryManagement.Api.Contracts;
using LibraryManagement.Domain.Loans;
using LibraryManagement.Domain.Members;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/members")]
public sealed class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateMemberRequest request, CancellationToken cancellationToken)
    {
        var memberId = await _mediator.Send(new CreateMemberCommand(request.Name, request.Profile), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = memberId }, memberId);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MemberDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var member = await _mediator.Send(new GetMemberByIdQuery(id), cancellationToken);
        return Ok(member);
    }

    [HttpGet("{id:guid}/penalties")]
    public async Task<ActionResult<MemberPenaltiesResult>> GetPenalties(Guid id, CancellationToken cancellationToken)
    {
        var penalties = await _mediator.Send(new GetMemberPenaltiesQuery(id), cancellationToken);
        return Ok(penalties);
    }
}
