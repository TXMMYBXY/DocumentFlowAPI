using System.Security.Claims;
using DocumentFlowAPI.Interfaces.Repositories;

namespace DocumentFlowAPI.Services.User;

public static class UserIdentity
{
    private static IServiceProvider _serviceProvider;
    public static void Initialize(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public static Models.User? User
    {
        get
        {
            if (_serviceProvider == null)
            {
                return null;
            }

            try
            {
                var httpContext = _serviceProvider.GetRequiredService<IHttpContextAccessor>();

                if (httpContext.HttpContext == null)
                {
                    return null;
                }

                var user = httpContext.HttpContext.User;

                if (user?.Identity?.IsAuthenticated != true)
                {
                    return null;
                }

                var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return null;
                }

                using var scope = _serviceProvider.CreateScope();
                var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
                var userModel = userRepository.GetUserByIdAsync(int.Parse(userId)).Result;

                return userModel;
            }
            catch
            {
                return null;
            }
        }
    }
}
