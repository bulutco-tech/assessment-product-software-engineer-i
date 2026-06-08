namespace Hrms.Application.OvertimeRequests;

public sealed record RejectOvertimeRequestCommand(
    int ManagerId,
    string Reason);
