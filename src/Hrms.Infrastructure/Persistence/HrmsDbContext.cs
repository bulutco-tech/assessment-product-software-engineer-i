using Hrms.Application.Abstractions;
using Hrms.Domain.Employees;
using Microsoft.EntityFrameworkCore;

namespace Hrms.Infrastructure.Persistence;

public sealed class HrmsDbContext(DbContextOptions<HrmsDbContext> options)
    : DbContext(options), IHrmsDbContext
{
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("Employees");
            entity.HasKey(employee => employee.Id);
            entity.Property(employee => employee.FullName)
                .HasMaxLength(200)
                .IsRequired();

            entity.HasIndex(employee => employee.ManagerId);

            entity.HasData(
                new Employee { Id = 1, FullName = "Manager", ManagerId = null },
                new Employee { Id = 2, FullName = "Employee", ManagerId = 1 });
        });
    }
}
