using Hrms.Application.Common.Exceptions;
using Hrms.Application.OvertimeRequests;
using Hrms.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace Hrms.Application.Tests.OvertimeRequests;

public sealed class OvertimeRequestServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Fail_When_EndTime_Is_Before_StartTime()
    {
        var service = CreateService();
        var command = ValidCreateCommand() with
        {
            StartTime = new TimeOnly(18, 0),
            EndTime = new TimeOnly(17, 0)
        };

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.CreateAsync(command));
    }

    [Fact]
    public async Task CreateAsync_Should_Fail_When_Duration_Is_Less_Than_30_Minutes()
    {
        var service = CreateService();
        var command = ValidCreateCommand() with
        {
            StartTime = new TimeOnly(18, 0),
            EndTime = new TimeOnly(18, 20)
        };

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.CreateAsync(command));
    }

    [Fact]
    public async Task CreateAsync_Should_Fail_When_Request_Overlaps()
    {
        var service = CreateService();

        await service.CreateAsync(ValidCreateCommand() with
        {
            StartTime = new TimeOnly(18, 0),
            EndTime = new TimeOnly(20, 0)
        });

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.CreateAsync(ValidCreateCommand() with
            {
                StartTime = new TimeOnly(19, 0),
                EndTime = new TimeOnly(21, 0)
            }));
    }

    [Fact]
    public async Task CreateAsync_Should_Create_Pending_Request_When_Input_Is_Valid()
    {
        var service = CreateService();

        var result = await service.CreateAsync(ValidCreateCommand());

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(2, result.EmployeeId);
        Assert.Equal("Pending", result.Status);
    }

    [Fact]
    public async Task ApproveAsync_Should_Return_Approved_Status()
    {
        var service = CreateService();
        var created = await service.CreateAsync(ValidCreateCommand());

        var approved = await service.ApproveAsync(
            created.Id,
            new ApproveOvertimeRequestCommand(ManagerId: 1, IdempotencyKey: "approval-test"));

        Assert.Equal("Approved", approved.Status);
    }

    [Fact]
    public async Task ApproveAsync_Should_Not_Allow_Second_Approval()
    {
        var service = CreateService();
        var created = await service.CreateAsync(ValidCreateCommand());

        await service.ApproveAsync(
            created.Id,
            new ApproveOvertimeRequestCommand(ManagerId: 1, IdempotencyKey: "approval-once"));

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.ApproveAsync(
                created.Id,
                new ApproveOvertimeRequestCommand(ManagerId: 1, IdempotencyKey: "approval-twice")));
    }

    [Fact]
    public async Task RejectAsync_Should_Require_Reason()
    {
        var service = CreateService();
        var created = await service.CreateAsync(ValidCreateCommand());

        await Assert.ThrowsAsync<BusinessRuleException>(() =>
            service.RejectAsync(
                created.Id,
                new RejectOvertimeRequestCommand(ManagerId: 1, Reason: " ")));
    }

    private static OvertimeRequestService CreateService()
    {
        var options = new DbContextOptionsBuilder<HrmsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var dbContext = new HrmsDbContext(options);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return new OvertimeRequestService(
            dbContext,
            NullLogger<OvertimeRequestService>.Instance);
    }

    private static CreateOvertimeRequestCommand ValidCreateCommand()
    {
        return new CreateOvertimeRequestCommand(
            EmployeeId: 2,
            Date: DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)),
            StartTime: new TimeOnly(18, 0),
            EndTime: new TimeOnly(19, 0),
            IdempotencyKey: "create-test");
    }
}
