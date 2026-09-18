namespace LibraryManagement.Domain.Members;

public sealed class Member
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public MemberProfile Profile { get; private set; }

    private Member(Guid id, string name, MemberProfile profile)
    {
        Id = id;
        Name = name;
        Profile = profile;
    }

    public static Member Create(string name, MemberProfile profile) =>
        new(Guid.NewGuid(), name, profile);

    public static Member Load(Guid id, string name, MemberProfile profile) =>
        new(id, name, profile);
}
