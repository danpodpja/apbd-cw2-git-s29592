namespace apbd_cw2_git_s29592.Domain.Users;

public class Employee : User
{
    public string Department { get; set; }
    public string EmployeeNumber { get; set; }

    public Employee(string firstName, string lastName, string department, string employeeNumber)
        : base(firstName, lastName)
    {
        Department = department;
        EmployeeNumber = employeeNumber;
    }

    public override string GetUserType() => "Employee";

    public override string ToString() =>
        $"{base.ToString()} | Wydział: {Department}, Nr: {EmployeeNumber}";
}