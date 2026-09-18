using FluentValidation;

namespace LibraryManagement.Domain.Loans;

public sealed class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
{
    public BorrowBookCommandValidator()
    {
        RuleFor(c => c.MemberId).NotEmpty();
        RuleFor(c => c.BookId).NotEmpty();
    }
}
