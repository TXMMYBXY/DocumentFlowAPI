using AutoMapper;
using DocumentFlowAPI.Interfaces.Repositories;
using DocumentFlowAPI.Interfaces.Services;
using DocumentFlowAPI.Services.Personal.Dto;

namespace DocumentFlowAPI.Services.Personal;

public class PersonalAccountService : IPersonalAccountService
{
    private readonly IMapper _mapper;
    private readonly IPersonalAccountRepository _personalAccountRepository;

    public PersonalAccountService(IMapper mapper, IPersonalAccountRepository personalAccountRepository)
    {
        _mapper = mapper;
        _personalAccountRepository = personalAccountRepository;
    }
    
    public async Task<GetPersonDto> GetPersonalInfoAsync(int personId)
    {
        var person = await _personalAccountRepository.GetPersonalInfoAsync(personId);
        var personDto = _mapper.Map<GetPersonDto>(person);
        
        return personDto;
    }
}