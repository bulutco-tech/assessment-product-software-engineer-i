namespace Hrms.Domain.Employees;

public sealed class Employee
{
    public int Id { get; set; }

    public required string FullName { get; set; }

    public int? ManagerId { get; set; }
}
