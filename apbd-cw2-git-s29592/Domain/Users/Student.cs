namespace apbd_cw2_git_s29592.Domain.Users;

public class Student : User
{
    public string StudentIndex { get; set; }

    public Student(string firstName, string lastName, string studentIndex)
        : base(firstName, lastName)
    {
        StudentIndex = studentIndex;
    }

    public override string GetUserType() => "Student";

    public override string ToString() =>
        $"{base.ToString()} | Nr indeksu: {StudentIndex}";
}