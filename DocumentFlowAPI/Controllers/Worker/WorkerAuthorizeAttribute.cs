using Microsoft.AspNetCore.Authorization;

namespace DocumentFlowAPI.Controllers.Worker;

public class WorkerAuthorizeAttribute : AuthorizeAttribute
{
    public WorkerAuthorizeAttribute()
    {
        AuthenticationSchemes = Middleware.WorkerAuthenticationHandler.SchemeName;
        Roles = "Worker";
    }
}
