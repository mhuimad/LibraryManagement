using LibraryManagement.Domain.Abstractions;

namespace LibraryManagement.Domain.Members;

public sealed record CreateMemberCommand(string Name, string Profile) : ICommand<Guid>;
