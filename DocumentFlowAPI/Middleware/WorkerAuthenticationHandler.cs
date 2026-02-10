using System.Security.Claims;
using System.Text.Encodings.Web;
using DocumentFlowAPI.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace DocumentFlowAPI.Middleware;

public class WorkerAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string SchemeName = "WorkerScheme";
    private const string WorkerHeader = "X-Worker-Api-Key"; // Кастомный загловок для запросов от службы
    private readonly WorkerSettings _workerSettings;

    public WorkerAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock,
        IOptions<WorkerSettings> workerSettings) 
        : base(options, logger, encoder, clock)
    {
        _workerSettings = workerSettings.Value;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(WorkerHeader, out var apiKey))
        {
            return Task.FromResult(AuthenticateResult.Fail("Missing API key"));
        }

        if (apiKey != _workerSettings.ApiKey)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid API key"));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Worker"),
            new Claim(ClaimTypes.Role, "Worker")
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
