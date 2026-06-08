namespace Hrms.Application.OvertimeRequests;

public sealed record CreateOvertimeRequestCommand(
    int EmployeeId,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime,
    string? IdempotencyKey = null);
