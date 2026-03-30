namespace apbd_cw2_git_s29592.Domain.Users;

public abstract class User
{
    public Guid   Id        { get; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName  { get; set; }
    public string FullName  => $"{FirstName} {LastName}";

    protected User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName  = lastName;
    }

    public abstract string GetUserType();

    public override string ToString() =>
        $"[{Id.ToString()[..8]}] {GetUserType()}: {FullName}";
}