using FluentValidation;

namespace LibraryManagement.Domain.Members;

public sealed class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty();
        RuleFor(c => c.Profile).NotEmpty().Must(p => p is "Standard" or "Student");
    }
}
