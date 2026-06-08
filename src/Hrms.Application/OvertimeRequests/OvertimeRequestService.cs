using Hrms.Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Hrms.Application.OvertimeRequests;

public sealed class OvertimeRequestService(
    IHrmsDbContext dbContext,
    ILogger<OvertimeRequestService> logger) : IOvertimeRequestService
{
    public Task<OvertimeRequestResult> CreateAsync(
        CreateOvertimeRequestCommand command,
        CancellationToken cancellationToken = default)
    {

        throw new NotImplementedException("Candidate must design and implement the overtime request model and creation workflow.");
    }

    public Task<OvertimeRequestResult> ApproveAsync(
        Guid id,
        ApproveOvertimeRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Candidate must design and implement the approval workflow.");
    }

    public Task<OvertimeRequestResult> RejectAsync(
        Guid id,
        RejectOvertimeRequestCommand command,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException("Candidate must design and implement the rejection workflow.");
    }
}
