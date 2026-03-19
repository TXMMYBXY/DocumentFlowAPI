using AutoMapper;
using DocumentFlowAPI.Controllers.PersonalAccount.ViewModels;
using DocumentFlowAPI.Services.Personal.Dto;

namespace DocumentFlowAPI.Controllers.PersonalAccount.MappingProfile;

public class PersonalAccountMappingProfile : Profile
{
    public PersonalAccountMappingProfile()
    {
        //Profiles for GET
        CreateMap<GetPersonDto, GetPersonViewModel>();
        
        CreateMap<PersonDto, GetPersonDto>();

        CreateMap<GetLoginTimesDto, GetLoginTimesViewModel>();
        
        CreateMap<LoginTimeDto, GetLoginTimesDto>();
            
        //Profiles for PATCH
        CreateMap<ChangePasswordViewModel, ChangePasswordDto>();
    }
}