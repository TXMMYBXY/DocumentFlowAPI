using DocumentFlowAPI.Interfaces.Base;
using DocumentFlowAPI.Services.Personal.Dto;

namespace DocumentFlowAPI.Interfaces.Repositories;

public interface IPersonalAccountRepository : IBaseRepository<Models.User>
{
    Task<PersonDto> GetPersonalInfo(int personId);
}