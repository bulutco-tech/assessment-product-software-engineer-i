namespace Hrms.Application.OvertimeRequests;

public sealed record ApproveOvertimeRequestCommand(
    int ManagerId,
    string? IdempotencyKey = null);
