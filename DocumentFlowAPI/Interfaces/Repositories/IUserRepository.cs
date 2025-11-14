using DocumentFlowAPI.Base;

namespace DocumentFlowAPI.Interfaces.Repositories
{
    public interface IUserRepository : IBaseRepository<Models.User>
    {
        Task CreateNewUserAsync(Models.User userModel);
    }
}