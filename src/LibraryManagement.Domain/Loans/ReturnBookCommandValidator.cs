using FluentValidation;

namespace LibraryManagement.Domain.Loans;

public sealed class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
{
    public ReturnBookCommandValidator()
    {
        RuleFor(c => c.LoanId).NotEmpty();
    }
}
