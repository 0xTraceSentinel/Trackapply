using Microsoft.Extensions.DependencyInjection;
using Trackapply.Application.JobApplications.Services;

namespace Trackapply.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJobApplicationService, JobApplicationService>();

        return services;
    }
}
