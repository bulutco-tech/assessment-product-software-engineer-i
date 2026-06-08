namespace Hrms.Application.OvertimeRequests;

public interface IOvertimeRequestService
{
    Task<OvertimeRequestResult> CreateAsync(
        CreateOvertimeRequestCommand command,
        CancellationToken cancellationToken = default);

    Task<OvertimeRequestResult> ApproveAsync(
        Guid id,
        ApproveOvertimeRequestCommand command,
        CancellationToken cancellationToken = default);

    Task<OvertimeRequestResult> RejectAsync(
        Guid id,
        RejectOvertimeRequestCommand command,
        CancellationToken cancellationToken = default);
}
