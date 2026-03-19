using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace DocumentFlowAPI.Controllers.Auth;

public class AuthorizeByRoleIdAttribute : AuthorizeAttribute, IAuthorizationFilter
{
    private readonly int[] _allowedRoles;
    
    public AuthorizeByRoleIdAttribute(params int[] allowedRoles)
    {
        _allowedRoles = allowedRoles;
    }
    
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var claimIsActive = context.HttpContext.User.FindFirst("IsActive").Value;
        var claimRoleId = context.HttpContext.User.FindFirst("RoleId").Value;

        if (string.IsNullOrEmpty(claimIsActive) && string.IsNullOrEmpty(claimRoleId))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!bool.TryParse(claimIsActive, out bool userIsActive) 
            & !int.TryParse(claimRoleId, out int userRoleId))
        {
            context.Result = new ForbidResult();
            return;
        }

        if (!userIsActive && !_allowedRoles.Contains(userRoleId))
        {
            context.Result = new ForbidResult();
        }

    }
}
