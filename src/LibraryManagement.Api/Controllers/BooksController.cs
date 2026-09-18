using LibraryManagement.Api.Contracts;
using LibraryManagement.Domain.Books;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers;

[ApiController]
[Route("api/books")]
public sealed class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateBookRequest request, CancellationToken cancellationToken)
    {
        var bookId = await _mediator.Send(new CreateBookCommand(request.Title, request.Author, request.TotalCopies), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = bookId }, bookId);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<BookDto>>> GetAll(CancellationToken cancellationToken)
    {
        var books = await _mediator.Send(new GetBooksQuery(), cancellationToken);
        return Ok(books);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BookDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var book = await _mediator.Send(new GetBookByIdQuery(id), cancellationToken);
        return Ok(book);
    }
}
