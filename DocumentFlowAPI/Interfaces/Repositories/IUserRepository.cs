using DocumentFlowAPI.Base;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace DocumentFlowAPI.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<Models.User>
    {
        Task CreateNewUserAsync(Models.User userModel);
        Task<Models.User> GetUserByIdAsync(int userId);
        Models.User UpdateUserStatus(Models.User user);
        Models.User UpdateUserInfo(Models.User user);
    }
}