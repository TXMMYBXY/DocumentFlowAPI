using DocumentFlowAPI.Configuration;
using DocumentFlowAPI.Middleware;
using Microsoft.AspNetCore.Authentication;

namespace DocumentFlowAPI.Services.WorkerTask;

public static class WorkerAuthExtensions
{
    public static IServiceCollection AddWorkerAuth(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<WorkerSettings>(
            configuration.GetSection(nameof(WorkerSettings)));

        services
            .AddAuthentication()
            .AddScheme<AuthenticationSchemeOptions, WorkerAuthenticationHandler>(
                WorkerAuthenticationHandler.SchemeName,
                _ => { });

        return services;
    }
}
