using Hrms.Domain.Employees;
using Hrms.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class EmployeesController(HrmsDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Employee>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Employee>>> GetAsync(
        CancellationToken cancellationToken)
    {
        var employees = await dbContext.Employees
            .AsNoTracking()
            .OrderBy(employee => employee.Id)
            .ToListAsync(cancellationToken);

        return Ok(employees);
    }
}
