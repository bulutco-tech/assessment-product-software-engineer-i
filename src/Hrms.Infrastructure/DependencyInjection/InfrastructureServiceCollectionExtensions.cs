using Hrms.Application.Abstractions;
using Hrms.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hrms.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddHrmsInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Hrms")
            ?? "Data Source=hrms.db";

        services.AddDbContext<HrmsDbContext>(options =>
            options.UseSqlite(connectionString));

        services.AddScoped<IHrmsDbContext>(provider =>
            provider.GetRequiredService<HrmsDbContext>());

        return services;
    }
}
