using Hrms.Application.OvertimeRequests;
using Microsoft.Extensions.DependencyInjection;

namespace Hrms.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddHrmsApplication(this IServiceCollection services)
    {
        services.AddScoped<IOvertimeRequestService, OvertimeRequestService>();

        return services;
    }
}
