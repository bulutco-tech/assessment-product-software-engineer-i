namespace Hrms.Application.OvertimeRequests;

public sealed record OvertimeRequestResult(
    Guid Id,
    int EmployeeId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    decimal DurationHours,
    string Status);
