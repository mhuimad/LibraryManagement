using FluentValidation;

namespace LibraryManagement.Domain.Books;

public sealed class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
{
    public CreateBookCommandValidator()
    {
        RuleFor(c => c.Title).NotEmpty();
        RuleFor(c => c.Author).NotEmpty();
        RuleFor(c => c.TotalCopies).GreaterThan(0);
    }
}
