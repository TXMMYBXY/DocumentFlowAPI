using DocumentFlowAPI.Base;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<Models.User>
{
    Task CreateNewUserAsync(Models.User userModel);
    Task<Models.User> GetUserByIdAsync(int userId);
    Models.User UpdateUserStatus(Models.User userModel);
    Models.User UpdateUser(Models.User userModel);
    Task<Models.User> GetUserByLoginAsync(string login);
    Task<bool> IsUserAlreadyExists(string login);
}