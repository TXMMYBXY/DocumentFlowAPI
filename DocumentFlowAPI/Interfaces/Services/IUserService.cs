using DocumentFlowAPI.Services.User.Dto;

namespace DocumentFlowAPI.Interfaces.Services;

public interface IUserService
{
    Task CreateNewUserAsync(NewUserDto newUserDto);
}